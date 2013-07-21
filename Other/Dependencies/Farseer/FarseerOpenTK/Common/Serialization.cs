using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using OpenTK;

namespace FarseerPhysics.Common
{
    public static class WorldSerializer
    {
        public static void Serialize(World world, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                new WorldXmlSerializer().Serialize(world, fs);
            }
        }

        public static void Deserialize(World world, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                new WorldXmlDeserializer().Deserialize(world, fs);
            }
        }

        public static World Deserialize(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return new WorldXmlDeserializer().Deserialize(fs);
            }
        }
    }

    ///<summary>
    ///</summary>
    public class WorldXmlSerializer
    {
        private List<Body> _bodies = new List<Body>();
        private List<Fixture> _serializedFixtures = new List<Fixture>();
        private List<Shape> _serializedShapes = new List<Shape>();
        private XmlWriter _writer;

        private void SerializeShape(Shape shape)
        {
            _writer.WriteStartElement("Shape");
            _writer.WriteAttributeString("Type", shape.ShapeType.ToString());

            switch (shape.ShapeType)
            {
                case ShapeType.Circle:
                    {
                        CircleShape circle = (CircleShape)shape;

                        _writer.WriteElementString("Radius", circle.Radius.ToString());

                        WriteElement("Position", circle.Position);
                    }
                    break;
                case ShapeType.Polygon:
                    {
                        PolygonShape poly = (PolygonShape)shape;

                        _writer.WriteStartElement("Vertices");
                        foreach (Vector2 v in poly.Vertices)
                            WriteElement("Vertex", v);
                        _writer.WriteEndElement();

                        WriteElement("Centroid", poly.MassData.Centroid);
                    }
                    break;
                case ShapeType.Edge:
                    {
                        EdgeShape poly = (EdgeShape)shape;
                        WriteElement("Vertex1", poly.Vertex1);
                        WriteElement("Vertex2", poly.Vertex2);
                    }
                    break;
                default:
                    throw new Exception();
            }

            _writer.WriteEndElement();
        }

        private void SerializeFixture(Fixture fixture)
        {
            _writer.WriteStartElement("Fixture");
            _writer.WriteElementString("Shape", FindShapeIndex(fixture.Shape).ToString());
            _writer.WriteElementString("Density", fixture.Shape.Density.ToString());

            _writer.WriteStartElement("FilterData");
            _writer.WriteElementString("CategoryBits", ((int)fixture.CollisionCategories).ToString());
            _writer.WriteElementString("MaskBits", ((int)fixture.CollidesWith).ToString());
            _writer.WriteElementString("GroupIndex", fixture.CollisionGroup.ToString());
            _writer.WriteEndElement();

            _writer.WriteElementString("Friction", fixture.Friction.ToString());
            _writer.WriteElementString("IsSensor", fixture.IsSensor.ToString());
            _writer.WriteElementString("Restitution", fixture.Restitution.ToString());

            if (fixture.UserData != null)
            {
                _writer.WriteStartElement("UserData");
                WriteDynamicType(fixture.UserData.GetType(), fixture.UserData);
                _writer.WriteEndElement();
            }

            _writer.WriteEndElement();
        }

        private void SerializeBody(Body body)
        {
            _writer.WriteStartElement("Body");
            _writer.WriteAttributeString("Type", body.BodyType.ToString());

            _writer.WriteElementString("Active", body.Enabled.ToString());
            _writer.WriteElementString("AllowSleep", body.SleepingAllowed.ToString());
            _writer.WriteElementString("Angle", body.Rotation.ToString());
            _writer.WriteElementString("AngularDamping", body.AngularDamping.ToString());
            _writer.WriteElementString("AngularVelocity", body.AngularVelocity.ToString());
            _writer.WriteElementString("Awake", body.Awake.ToString());
            _writer.WriteElementString("Bullet", body.IsBullet.ToString());
            _writer.WriteElementString("FixedRotation", body.FixedRotation.ToString());
            _writer.WriteElementString("LinearDamping", body.LinearDamping.ToString());
            WriteElement("LinearVelocity", body.LinearVelocity);
            WriteElement("Position", body.Position);

            if (body.UserData != null)
            {
                _writer.WriteStartElement("UserData");
                WriteDynamicType(body.UserData.GetType(), body.UserData);
                _writer.WriteEndElement();
            }

            _writer.WriteStartElement("Fixtures");
            for (int i = 0; i < body.FixtureList.Count; i++)
            {
                _writer.WriteElementString("ID", FindFixtureIndex(body.FixtureList[i]).ToString());
            }

            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void SerializeJoint(Joint joint)
        {
            if (joint.IsFixedType())
                return;

            _writer.WriteStartElement("Joint");

            _writer.WriteAttributeString("Type", joint.JointType.ToString());

            WriteElement("BodyA", FindBodyIndex(joint.BodyA));
            WriteElement("BodyB", FindBodyIndex(joint.BodyB));

            WriteElement("CollideConnected", joint.CollideConnected);

            WriteElement("Breakpoint", joint.Breakpoint);

            if (joint.UserData != null)
            {
                _writer.WriteStartElement("UserData");
                WriteDynamicType(joint.UserData.GetType(), joint.UserData);
                _writer.WriteEndElement();
            }

            switch (joint.JointType)
            {
                case JointType.Distance:
                    {
                        DistanceJoint djd = (DistanceJoint)joint;

                        WriteElement("DampingRatio", djd.DampingRatio);
                        WriteElement("FrequencyHz", djd.Frequency);
                        WriteElement("Length", djd.Length);
                        WriteElement("LocalAnchorA", djd.LocalAnchorA);
                        WriteElement("LocalAnchorB", djd.LocalAnchorB);
                    }
                    break;
                case JointType.Friction:
                    {
                        FrictionJoint fjd = (FrictionJoint)joint;
                        WriteElement("LocalAnchorA", fjd.LocalAnchorA);
                        WriteElement("LocalAnchorB", fjd.LocalAnchorB);
                        WriteElement("MaxForce", fjd.MaxForce);
                        WriteElement("MaxTorque", fjd.MaxTorque);
                    }
                    break;
                case JointType.Gear:
                    throw new Exception("Gear joint not supported by serialization");
                case JointType.Line:
                    {
                        LineJoint ljd = (LineJoint)joint;

                        WriteElement("EnableMotor", ljd.MotorEnabled);
                        WriteElement("LocalAnchorA", ljd.LocalAnchorA);
                        WriteElement("LocalAnchorB", ljd.LocalAnchorB);
                        WriteElement("MotorSpeed", ljd.MotorSpeed);
                        WriteElement("DampingRatio", ljd.DampingRatio);
                        WriteElement("MaxMotorTorque", ljd.MaxMotorTorque);
                        WriteElement("FrequencyHz", ljd.Frequency);
                        WriteElement("LocalXAxis", ljd.LocalXAxis);
                    }
                    break;
                case JointType.Prismatic:
                    {
                        PrismaticJoint pjd = (PrismaticJoint)joint;

                        //NOTE: Does not conform with Box2DScene

                        WriteElement("EnableLimit", pjd.LimitEnabled);
                        WriteElement("EnableMotor", pjd.MotorEnabled);
                        WriteElement("LocalAnchorA", pjd.LocalAnchorA);
                        WriteElement("LocalAnchorB", pjd.LocalAnchorB);
                        WriteElement("LocalXAxis1", pjd.LocalXAxis1);
                        WriteElement("LowerTranslation", pjd.LowerLimit);
                        WriteElement("UpperTranslation", pjd.UpperLimit);
                        WriteElement("MaxMotorForce", pjd.MaxMotorForce);
                        WriteElement("MotorSpeed", pjd.MotorSpeed);
                    }
                    break;
                case JointType.Pulley:
                    {
                        PulleyJoint pjd = (PulleyJoint)joint;

                        WriteElement("GroundAnchorA", pjd.GroundAnchorA);
                        WriteElement("GroundAnchorB", pjd.GroundAnchorB);
                        WriteElement("LengthA", pjd.LengthA);
                        WriteElement("LengthB", pjd.LengthB);
                        WriteElement("LocalAnchorA", pjd.LocalAnchorA);
                        WriteElement("LocalAnchorB", pjd.LocalAnchorB);
                        WriteElement("MaxLengthA", pjd.MaxLengthA);
                        WriteElement("MaxLengthB", pjd.MaxLengthB);
                        WriteElement("Ratio", pjd.Ratio);
                    }
                    break;
                case JointType.Revolute:
                    {
                        RevoluteJoint rjd = (RevoluteJoint)joint;

                        WriteElement("EnableLimit", rjd.LimitEnabled);
                        WriteElement("EnableMotor", rjd.MotorEnabled);
                        WriteElement("LocalAnchorA", rjd.LocalAnchorA);
                        WriteElement("LocalAnchorB", rjd.LocalAnchorB);
                        WriteElement("LowerAngle", rjd.LowerLimit);
                        WriteElement("MaxMotorTorque", rjd.MaxMotorTorque);
                        WriteElement("MotorSpeed", rjd.MotorSpeed);
                        WriteElement("ReferenceAngle", rjd.ReferenceAngle);
                        WriteElement("UpperAngle", rjd.UpperLimit);
                    }
                    break;
                case JointType.Weld:
                    {
                        WeldJoint wjd = (WeldJoint)joint;

                        WriteElement("LocalAnchorA", wjd.LocalAnchorA);
                        WriteElement("LocalAnchorB", wjd.LocalAnchorB);
                    }
                    break;
                //
                // Not part of Box2DScene
                //
                case JointType.Rope:
                    {
                        RopeJoint rjd = (RopeJoint)joint;

                        WriteElement("LocalAnchorA", rjd.LocalAnchorA);
                        WriteElement("LocalAnchorB", rjd.LocalAnchorB);
                        WriteElement("MaxLength", rjd.MaxLength);
                    }
                    break;
                case JointType.Angle:
                    {
                        AngleJoint aj = (AngleJoint)joint;
                        WriteElement("BiasFactor", aj.BiasFactor);
                        WriteElement("MaxImpulse", aj.MaxImpulse);
                        WriteElement("Softness", aj.Softness);
                        WriteElement("TargetAngle", aj.TargetAngle);
                    }
                    break;
                case JointType.Slider:
                    {
                        SliderJoint sliderJoint = (SliderJoint)joint;
                        WriteElement("DampingRatio", sliderJoint.DampingRatio);
                        WriteElement("FrequencyHz", sliderJoint.Frequency);
                        WriteElement("MaxLength", sliderJoint.MaxLength);
                        WriteElement("MinLength", sliderJoint.MinLength);
                        WriteElement("LocalAnchorA", sliderJoint.LocalAnchorA);
                        WriteElement("LocalAnchorB", sliderJoint.LocalAnchorB);
                    }
                    break;
                default:
                    throw new Exception("Joint not supported");
            }

            _writer.WriteEndElement();
        }

        private void WriteDynamicType(Type type, object val)
        {
            _writer.WriteElementString("Type", type.FullName);

            _writer.WriteStartElement("Value");
            XmlSerializer serializer = new XmlSerializer(type);
            XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");
            serializer.Serialize(_writer, val, xmlnsEmpty);
            _writer.WriteEndElement();
        }

        private void WriteElement(string name, Vector2 vec)
        {
            _writer.WriteElementString(name, vec.X + " " + vec.Y);
        }

        private void WriteElement(string name, int val)
        {
            _writer.WriteElementString(name, val.ToString());
        }

        private void WriteElement(string name, bool val)
        {
            _writer.WriteElementString(name, val.ToString());
        }

        private void WriteElement(string name, float val)
        {
            _writer.WriteElementString(name, val.ToString());
        }

        public void Serialize(World world, Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = true;

            _writer = XmlWriter.Create(stream, settings);

            _writer.WriteStartElement("World");
            _writer.WriteAttributeString("Version", "2");
            WriteElement("Gravity", world.Gravity);

            _writer.WriteStartElement("Shapes");

            for (int i = 0; i < world.BodyList.Count; i++)
            {
                Body body = world.BodyList[i];
                for (int j = 0; j < body.FixtureList.Count; j++)
                {
                    Fixture fixture = body.FixtureList[j];

                    bool alreadyThere = false;
                    for (int k = 0; k < _serializedShapes.Count; k++)
                    {
                        Shape s2 = _serializedShapes[k];
                        if (fixture.Shape.CompareTo(s2))
                        {
                            alreadyThere = true;
                            break;
                        }
                    }

                    if (!alreadyThere)
                    {
                        SerializeShape(fixture.Shape);
                        _serializedShapes.Add(fixture.Shape);
                    }
                }
            }

            _writer.WriteEndElement();
            _writer.WriteStartElement("Fixtures");


            for (int i = 0; i < world.BodyList.Count; i++)
            {
                Body body = world.BodyList[i];
                for (int j = 0; j < body.FixtureList.Count; j++)
                {
                    Fixture fixture = body.FixtureList[j];
                    bool alreadyThere = false;
                    for (int k = 0; k < _serializedFixtures.Count; k++)
                    {
                        Fixture f2 = _serializedFixtures[k];
                        if (fixture.CompareTo(f2))
                        {
                            alreadyThere = true;
                            break;
                        }
                    }

                    if (!alreadyThere)
                    {
                        SerializeFixture(fixture);
                        _serializedFixtures.Add(fixture);
                    }
                }
            }

            _writer.WriteEndElement();
            _writer.WriteStartElement("Bodies");

            for (int i = 0; i < world.BodyList.Count; i++)
            {
                Body body = world.BodyList[i];
                _bodies.Add(body);
                SerializeBody(body);
            }

            _writer.WriteEndElement();
            _writer.WriteStartElement("Joints");

            for (int i = 0; i < world.JointList.Count; i++)
            {
                Joint joint = world.JointList[i];
                SerializeJoint(joint);
            }

            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.Flush();
            _writer.Close();
        }

        private int FindBodyIndex(Body body)
        {
            for (int i = 0; i < _bodies.Count; ++i)
                if (_bodies[i] == body)
                    return i;

            return -1;
        }

        private int FindFixtureIndex(Fixture fixture)
        {
            for (int i = 0; i < _serializedFixtures.Count; ++i)
            {
                if (_serializedFixtures[i].CompareTo(fixture))
                    return i;
            }

            return -1;
        }

        private int FindShapeIndex(Shape shape)
        {
            for (int i = 0; i < _serializedShapes.Count; ++i)
            {
                if (_serializedShapes[i].CompareTo(shape))
                    return i;
            }

            return -1;
        }
    }

    public class WorldXmlDeserializer
    {
        private List<Body> _bodies = new List<Body>();
        private List<Fixture> _fixtures = new List<Fixture>();
        private List<Joint> _joints = new List<Joint>();
        private List<Shape> _shapes = new List<Shape>();

        public World Deserialize(Stream stream)
        {
            World world = new World(Vector2.Zero);
            Deserialize(world, stream);
            return world;
        }

        public void Deserialize(World world, Stream stream)
        {
            world.Clear();

            XMLFragmentElement root = XMLFragmentParser.LoadFromStream(stream);

            if (root.Name.ToLower() != "world")
                throw new Exception();

            foreach (XMLFragmentElement main in root.Elements)
            {
                if (main.Name.ToLower() == "gravity")
                {
                    world.Gravity = ReadVector(main);
                }
            }

            foreach (XMLFragmentElement shapeElement in root.Elements)
            {
                if (shapeElement.Name.ToLower() == "shapes")
                {
                    foreach (XMLFragmentElement n in shapeElement.Elements)
                    {
                        if (n.Name.ToLower() != "shape")
                            throw new Exception();

                        ShapeType type = (ShapeType)Enum.Parse(typeof(ShapeType), n.Attributes[0].Value, true);

                        switch (type)
                        {
                            case ShapeType.Circle:
                                {
                                    CircleShape shape = new CircleShape();

                                    foreach (XMLFragmentElement sn in n.Elements)
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "radius":
                                                shape.Radius = float.Parse(sn.Value);
                                                break;
                                            case "position":
                                                shape.Position = ReadVector(sn);
                                                break;
                                            default:
                                                throw new Exception();
                                        }
                                    }

                                    _shapes.Add(shape);
                                }
                                break;
                            case ShapeType.Polygon:
                                {
                                    PolygonShape shape = new PolygonShape();

                                    foreach (XMLFragmentElement sn in n.Elements)
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "vertices":
                                                {
                                                    List<Vector2> verts = new List<Vector2>();

                                                    foreach (XMLFragmentElement vert in sn.Elements)
                                                        verts.Add(ReadVector(vert));

                                                    shape.Set(new Vertices(verts.ToArray()));
                                                }
                                                break;
                                            case "centroid":
                                                shape.MassData.Centroid = ReadVector(sn);
                                                break;
                                        }
                                    }

                                    _shapes.Add(shape);
                                }
                                break;
                            case ShapeType.Edge:
                                {
                                    EdgeShape shape = new EdgeShape();
                                    foreach (XMLFragmentElement sn in n.Elements)
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "hasvertex0":
                                                shape.HasVertex0 = bool.Parse(sn.Value);
                                                break;
                                            case "hasvertex3":
                                                shape.HasVertex0 = bool.Parse(sn.Value);
                                                break;
                                            case "vertex0":
                                                shape.Vertex0 = ReadVector(sn);
                                                break;
                                            case "vertex1":
                                                shape.Vertex1 = ReadVector(sn);
                                                break;
                                            case "vertex2":
                                                shape.Vertex2 = ReadVector(sn);
                                                break;
                                            case "vertex3":
                                                shape.Vertex3 = ReadVector(sn);
                                                break;
                                            default:
                                                throw new Exception();
                                        }
                                    }
                                    _shapes.Add(shape);
                                }
                                break;
                        }
                    }
                }
            }

            foreach (XMLFragmentElement fixtureElement in root.Elements)
            {
                if (fixtureElement.Name.ToLower() == "fixtures")
                {
                    foreach (XMLFragmentElement n in fixtureElement.Elements)
                    {
                        Fixture fixture = new Fixture();

                        if (n.Name.ToLower() != "fixture")
                            throw new Exception();

                        foreach (XMLFragmentElement sn in n.Elements)
                        {
                            switch (sn.Name.ToLower())
                            {
                                case "shape":
                                    fixture.Shape = _shapes[int.Parse(sn.Value)];
                                    break;
                                case "density":
                                    fixture.Shape.Density = float.Parse(sn.Value);
                                    break;
                                case "filterdata":
                                    foreach (XMLFragmentElement ssn in sn.Elements)
                                    {
                                        switch (ssn.Name.ToLower())
                                        {
                                            case "categorybits":
                                                fixture._collisionCategories = (Category)int.Parse(ssn.Value);
                                                break;
                                            case "maskbits":
                                                fixture._collidesWith = (Category)int.Parse(ssn.Value);
                                                break;
                                            case "groupindex":
                                                fixture._collisionGroup = short.Parse(ssn.Value);
                                                break;
                                        }
                                    }

                                    break;
                                case "friction":
                                    fixture.Friction = float.Parse(sn.Value);
                                    break;
                                case "issensor":
                                    fixture.IsSensor = bool.Parse(sn.Value);
                                    break;
                                case "restitution":
                                    fixture.Restitution = float.Parse(sn.Value);
                                    break;
                                case "userdata":
                                    fixture.UserData = ReadSimpleType(sn, null, false);
                                    break;
                            }
                        }

                        _fixtures.Add(fixture);
                    }
                }
            }

            foreach (XMLFragmentElement bodyElement in root.Elements)
            {
                if (bodyElement.Name.ToLower() == "bodies")
                {
                    foreach (XMLFragmentElement n in bodyElement.Elements)
                    {
                        Body body = new Body(world);

                        if (n.Name.ToLower() != "body")
                            throw new Exception();

                        body.BodyType = (BodyType)Enum.Parse(typeof(BodyType), n.Attributes[0].Value, true);

                        foreach (XMLFragmentElement sn in n.Elements)
                        {
                            switch (sn.Name.ToLower())
                            {
                                case "active":
                                    if (bool.Parse(sn.Value))
                                        body.Flags |= BodyFlags.Enabled;
                                    else
                                        body.Flags &= ~BodyFlags.Enabled;
                                    break;
                                case "allowsleep":
                                    body.SleepingAllowed = bool.Parse(sn.Value);
                                    break;
                                case "angle":
                                    {
                                        Vector2 position = body.Position;
                                        body.SetTransformIgnoreContacts(ref position, float.Parse(sn.Value));
                                    }
                                    break;
                                case "angulardamping":
                                    body.AngularDamping = float.Parse(sn.Value);
                                    break;
                                case "angularvelocity":
                                    body.AngularVelocity = float.Parse(sn.Value);
                                    break;
                                case "awake":
                                    body.Awake = bool.Parse(sn.Value);
                                    break;
                                case "bullet":
                                    body.IsBullet = bool.Parse(sn.Value);
                                    break;
                                case "fixedrotation":
                                    body.FixedRotation = bool.Parse(sn.Value);
                                    break;
                                case "lineardamping":
                                    body.LinearDamping = float.Parse(sn.Value);
                                    break;
                                case "linearvelocity":
                                    body.LinearVelocity = ReadVector(sn);
                                    break;
                                case "position":
                                    {
                                        float rotation = body.Rotation;
                                        Vector2 position = ReadVector(sn);
                                        body.SetTransformIgnoreContacts(ref position, rotation);
                                    }
                                    break;
                                case "userdata":
                                    body.UserData = ReadSimpleType(sn, null, false);
                                    break;
                                case "fixtures":
                                    {
                                        foreach (XMLFragmentElement v in sn.Elements)
                                        {
                                            Fixture blueprint = _fixtures[int.Parse(v.Value)];
                                            Fixture f = new Fixture(body, blueprint.Shape);
                                            f.Restitution = blueprint.Restitution;
                                            f.UserData = blueprint.UserData;
                                            f.Friction = blueprint.Friction;
                                            f.CollidesWith = blueprint.CollidesWith;
                                            f.CollisionCategories = blueprint.CollisionCategories;
                                            f.CollisionGroup = blueprint.CollisionGroup;
                                        }
                                        break;
                                    }
                            }
                        }

                        _bodies.Add(body);
                    }
                }
            }

            foreach (XMLFragmentElement jointElement in root.Elements)
            {
                if (jointElement.Name.ToLower() == "joints")
                {
                    foreach (XMLFragmentElement n in jointElement.Elements)
                    {
                        Joint joint;

                        if (n.Name.ToLower() != "joint")
                            throw new Exception();

                        JointType type = (JointType)Enum.Parse(typeof(JointType), n.Attributes[0].Value, true);

                        int bodyAIndex = -1, bodyBIndex = -1;
                        bool collideConnected = false;
                        object userData = null;

                        foreach (XMLFragmentElement sn in n.Elements)
                        {
                            switch (sn.Name.ToLower())
                            {
                                case "bodya":
                                    bodyAIndex = int.Parse(sn.Value);
                                    break;
                                case "bodyb":
                                    bodyBIndex = int.Parse(sn.Value);
                                    break;
                                case "collideconnected":
                                    collideConnected = bool.Parse(sn.Value);
                                    break;
                                case "userdata":
                                    userData = ReadSimpleType(sn, null, false);
                                    break;
                            }
                        }

                        Body bodyA = _bodies[bodyAIndex];
                        Body bodyB = _bodies[bodyBIndex];

                        switch (type)
                        {
                            case JointType.Distance:
                                joint = new DistanceJoint();
                                break;
                            case JointType.Friction:
                                joint = new FrictionJoint();
                                break;
                            case JointType.Line:
                                joint = new LineJoint();
                                break;
                            case JointType.Prismatic:
                                joint = new PrismaticJoint();
                                break;
                            case JointType.Pulley:
                                joint = new PulleyJoint();
                                break;
                            case JointType.Revolute:
                                joint = new RevoluteJoint();
                                break;
                            case JointType.Weld:
                                joint = new WeldJoint();
                                break;
                            case JointType.Rope:
                                joint = new RopeJoint();
                                break;
                            case JointType.Angle:
                                joint = new AngleJoint();
                                break;
                            case JointType.Slider:
                                joint = new SliderJoint();
                                break;
                            case JointType.Gear:
                                throw new Exception("GearJoint is not supported.");
                            default:
                                throw new Exception("Invalid or unsupported joint.");
                        }

                        joint.CollideConnected = collideConnected;
                        joint.UserData = userData;
                        joint.BodyA = bodyA;
                        joint.BodyB = bodyB;
                        _joints.Add(joint);
                        world.AddJoint(joint);

                        foreach (XMLFragmentElement sn in n.Elements)
                        {
                            // check for specific nodes
                            switch (type)
                            {
                                case JointType.Distance:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "dampingratio":
                                                ((DistanceJoint)joint).DampingRatio = float.Parse(sn.Value);
                                                break;
                                            case "frequencyhz":
                                                ((DistanceJoint)joint).Frequency = float.Parse(sn.Value);
                                                break;
                                            case "length":
                                                ((DistanceJoint)joint).Length = float.Parse(sn.Value);
                                                break;
                                            case "localanchora":
                                                ((DistanceJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((DistanceJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Friction:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "localanchora":
                                                ((FrictionJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((FrictionJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                            case "maxforce":
                                                ((FrictionJoint)joint).MaxForce = float.Parse(sn.Value);
                                                break;
                                            case "maxtorque":
                                                ((FrictionJoint)joint).MaxTorque = float.Parse(sn.Value);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Line:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "enablemotor":
                                                ((LineJoint)joint).MotorEnabled = bool.Parse(sn.Value);
                                                break;
                                            case "localanchora":
                                                ((LineJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((LineJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                            case "motorspeed":
                                                ((LineJoint)joint).MotorSpeed = float.Parse(sn.Value);
                                                break;
                                            case "dampingratio":
                                                ((LineJoint)joint).DampingRatio = float.Parse(sn.Value);
                                                break;
                                            case "maxmotortorque":
                                                ((LineJoint)joint).MaxMotorTorque = float.Parse(sn.Value);
                                                break;
                                            case "frequencyhz":
                                                ((LineJoint)joint).Frequency = float.Parse(sn.Value);
                                                break;
                                            case "localxaxis":
                                                ((LineJoint)joint).LocalXAxis = ReadVector(sn);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Prismatic:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "enablelimit":
                                                ((PrismaticJoint)joint).LimitEnabled = bool.Parse(sn.Value);
                                                break;
                                            case "enablemotor":
                                                ((PrismaticJoint)joint).MotorEnabled = bool.Parse(sn.Value);
                                                break;
                                            case "localanchora":
                                                ((PrismaticJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((PrismaticJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                            case "local1axis1":
                                                ((PrismaticJoint)joint).LocalXAxis1 = ReadVector(sn);
                                                break;
                                            case "maxmotorforce":
                                                ((PrismaticJoint)joint).MaxMotorForce = float.Parse(sn.Value);
                                                break;
                                            case "motorspeed":
                                                ((PrismaticJoint)joint).MotorSpeed = float.Parse(sn.Value);
                                                break;
                                            case "lowertranslation":
                                                ((PrismaticJoint)joint).LowerLimit = float.Parse(sn.Value);
                                                break;
                                            case "uppertranslation":
                                                ((PrismaticJoint)joint).UpperLimit = float.Parse(sn.Value);
                                                break;
                                            case "referenceangle":
                                                ((PrismaticJoint)joint).ReferenceAngle = float.Parse(sn.Value);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Pulley:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "groundanchora":
                                                ((PulleyJoint)joint).GroundAnchorA = ReadVector(sn);
                                                break;
                                            case "groundanchorb":
                                                ((PulleyJoint)joint).GroundAnchorB = ReadVector(sn);
                                                break;
                                            case "lengtha":
                                                ((PulleyJoint)joint).LengthA = float.Parse(sn.Value);
                                                break;
                                            case "lengthb":
                                                ((PulleyJoint)joint).LengthB = float.Parse(sn.Value);
                                                break;
                                            case "localanchora":
                                                ((PulleyJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((PulleyJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                            case "maxlengtha":
                                                ((PulleyJoint)joint).MaxLengthA = float.Parse(sn.Value);
                                                break;
                                            case "maxlengthb":
                                                ((PulleyJoint)joint).MaxLengthB = float.Parse(sn.Value);
                                                break;
                                            case "ratio":
                                                ((PulleyJoint)joint).Ratio = float.Parse(sn.Value);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Revolute:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "enablelimit":
                                                ((RevoluteJoint)joint).LimitEnabled = bool.Parse(sn.Value);
                                                break;
                                            case "enablemotor":
                                                ((RevoluteJoint)joint).MotorEnabled = bool.Parse(sn.Value);
                                                break;
                                            case "localanchora":
                                                ((RevoluteJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((RevoluteJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                            case "maxmotortorque":
                                                ((RevoluteJoint)joint).MaxMotorTorque = float.Parse(sn.Value);
                                                break;
                                            case "motorspeed":
                                                ((RevoluteJoint)joint).MotorSpeed = float.Parse(sn.Value);
                                                break;
                                            case "lowerangle":
                                                ((RevoluteJoint)joint).LowerLimit = float.Parse(sn.Value);
                                                break;
                                            case "upperangle":
                                                ((RevoluteJoint)joint).UpperLimit = float.Parse(sn.Value);
                                                break;
                                            case "referenceangle":
                                                ((RevoluteJoint)joint).ReferenceAngle = float.Parse(sn.Value);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Weld:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "localanchora":
                                                ((WeldJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((WeldJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Rope:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "localanchora":
                                                ((RopeJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((RopeJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                            case "maxlength":
                                                ((RopeJoint)joint).MaxLength = float.Parse(sn.Value);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Gear:
                                    throw new Exception("Gear joint is unsupported");
                                case JointType.Angle:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "biasfactor":
                                                ((AngleJoint)joint).BiasFactor = float.Parse(sn.Value);
                                                break;
                                            case "maximpulse":
                                                ((AngleJoint)joint).MaxImpulse = float.Parse(sn.Value);
                                                break;
                                            case "softness":
                                                ((AngleJoint)joint).Softness = float.Parse(sn.Value);
                                                break;
                                            case "targetangle":
                                                ((AngleJoint)joint).TargetAngle = float.Parse(sn.Value);
                                                break;
                                        }
                                    }
                                    break;
                                case JointType.Slider:
                                    {
                                        switch (sn.Name.ToLower())
                                        {
                                            case "dampingratio":
                                                ((SliderJoint)joint).DampingRatio = float.Parse(sn.Value);
                                                break;
                                            case "frequencyhz":
                                                ((SliderJoint)joint).Frequency = float.Parse(sn.Value);
                                                break;
                                            case "maxlength":
                                                ((SliderJoint)joint).MaxLength = float.Parse(sn.Value);
                                                break;
                                            case "minlength":
                                                ((SliderJoint)joint).MinLength = float.Parse(sn.Value);
                                                break;
                                            case "localanchora":
                                                ((SliderJoint)joint).LocalAnchorA = ReadVector(sn);
                                                break;
                                            case "localanchorb":
                                                ((SliderJoint)joint).LocalAnchorB = ReadVector(sn);
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private Vector2 ReadVector(XMLFragmentElement node)
        {
            string[] values = node.Value.Split(' ');
            return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
        }

        private object ReadSimpleType(XMLFragmentElement node, Type type, bool outer)
        {
            if (type == null)
                return ReadSimpleType(node.Elements[1], Type.GetType(node.Elements[0].Value), outer);

            XmlSerializer serializer = new XmlSerializer(type);
            XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");

            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                {
                    writer.Write((outer) ? node.OuterXml : node.InnerXml);
                    writer.Flush();
                    stream.Position = 0;
                }
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Fragment;

                return serializer.Deserialize(XmlReader.Create(stream, settings));
            }
        }
    }

    #region XMLFragment

    public class XMLFragmentAttribute
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class XMLFragmentElement
    {
        private List<XMLFragmentAttribute> _attributes = new List<XMLFragmentAttribute>();
        private List<XMLFragmentElement> _elements = new List<XMLFragmentElement>();

        public IList<XMLFragmentElement> Elements
        {
            get { return _elements; }
        }

        public IList<XMLFragmentAttribute> Attributes
        {
            get { return _attributes; }
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string OuterXml { get; set; }

        public string InnerXml { get; set; }
    }

    public class XMLFragmentException : Exception
    {
        public XMLFragmentException()
        {
        }

        public XMLFragmentException(string message)
            : base(message)
        {
        }

        public XMLFragmentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class FileBuffer
    {
        public FileBuffer(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
                Buffer = sr.ReadToEnd();

            Position = 0;
        }

        public string Buffer { get; set; }

        public int Position { get; set; }

        public int Length
        {
            get { return Buffer.Length; }
        }

        public char Next
        {
            get
            {
                char c = Buffer[Position];
                Position++;
                return c;
            }
        }

        public char Peek
        {
            get { return Buffer[Position]; }
        }

        public bool EndOfBuffer
        {
            get { return Position == Length; }
        }
    }

    public class XMLFragmentParser
    {
        private static List<char> _punctuation = new List<char> { '/', '<', '>', '=' };
        private FileBuffer _buffer;
        private XMLFragmentElement _rootNode;

        public XMLFragmentParser(Stream stream)
        {
            Load(stream);
        }

        public XMLFragmentParser(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                Load(fs);
        }

        public XMLFragmentElement RootNode
        {
            get { return _rootNode; }
        }

        public void Load(Stream stream)
        {
            _buffer = new FileBuffer(stream);
        }

        public static XMLFragmentElement LoadFromFile(string fileName)
        {
            XMLFragmentParser x = new XMLFragmentParser(fileName);
            x.Parse();
            return x.RootNode;
        }

        public static XMLFragmentElement LoadFromStream(Stream stream)
        {
            XMLFragmentParser x = new XMLFragmentParser(stream);
            x.Parse();
            return x.RootNode;
        }

        private string NextToken()
        {
            string str = "";
            bool _done = false;

            while (true)
            {
                char c = _buffer.Next;

                if (_punctuation.Contains(c))
                {
                    if (str != "")
                    {
                        _buffer.Position--;
                        break;
                    }

                    _done = true;
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (str != "")
                        break;
                    else
                        continue;
                }

                str += c;

                if (_done)
                    break;
            }

            str = TrimControl(str);

            // Trim quotes from start and end
            if (str[0] == '\"')
                str = str.Remove(0, 1);

            if (str[str.Length - 1] == '\"')
                str = str.Remove(str.Length - 1, 1);

            return str;
        }

        private string PeekToken()
        {
            int oldPos = _buffer.Position;
            string str = NextToken();
            _buffer.Position = oldPos;
            return str;
        }

        private string ReadUntil(char c)
        {
            string str = "";

            while (true)
            {
                char ch = _buffer.Next;

                if (ch == c)
                {
                    _buffer.Position--;
                    break;
                }

                str += ch;
            }

            // Trim quotes from start and end
            if (str[0] == '\"')
                str = str.Remove(0, 1);

            if (str[str.Length - 1] == '\"')
                str = str.Remove(str.Length - 1, 1);

            return str;
        }

        private string TrimControl(string str)
        {
            string newStr = str;

            // Trim control characters
            int i = 0;
            while (true)
            {
                if (i == newStr.Length)
                    break;

                if (char.IsControl(newStr[i]))
                    newStr = newStr.Remove(i, 1);
                else
                    i++;
            }

            return newStr;
        }

        private string TrimTags(string outer)
        {
            int start = outer.IndexOf('>') + 1;
            int end = outer.LastIndexOf('<');

            return TrimControl(outer.Substring(start, end - start));
        }

        public XMLFragmentElement TryParseNode()
        {
            if (_buffer.EndOfBuffer)
                return null;

            int startOuterXml = _buffer.Position;
            string token = NextToken();

            if (token != "<")
                throw new XMLFragmentException("Expected \"<\", got " + token);

            XMLFragmentElement element = new XMLFragmentElement();
            element.Name = NextToken();

            while (true)
            {
                token = NextToken();

                if (token == ">")
                    break;
                else if (token == "/") // quick-exit case
                {
                    NextToken();

                    element.OuterXml =
                        TrimControl(_buffer.Buffer.Substring(startOuterXml, _buffer.Position - startOuterXml)).Trim();
                    element.InnerXml = "";

                    return element;
                }
                else
                {
                    XMLFragmentAttribute attribute = new XMLFragmentAttribute();
                    attribute.Name = token;
                    if ((token = NextToken()) != "=")
                        throw new XMLFragmentException("Expected \"=\", got " + token);
                    attribute.Value = NextToken();

                    element.Attributes.Add(attribute);
                }
            }

            while (true)
            {
                int oldPos = _buffer.Position; // for restoration below
                token = NextToken();

                if (token == "<")
                {
                    token = PeekToken();

                    if (token == "/") // finish element
                    {
                        NextToken(); // skip the / again
                        token = NextToken();
                        NextToken(); // skip >

                        element.OuterXml =
                            TrimControl(_buffer.Buffer.Substring(startOuterXml, _buffer.Position - startOuterXml)).Trim();
                        element.InnerXml = TrimTags(element.OuterXml);

                        if (token != element.Name)
                            throw new XMLFragmentException("Mismatched element pairs: \"" + element.Name + "\" vs \"" +
                                                           token + "\"");

                        break;
                    }
                    else
                    {
                        _buffer.Position = oldPos;
                        element.Elements.Add(TryParseNode());
                    }
                }
                else
                {
                    // value, probably
                    _buffer.Position = oldPos;
                    element.Value = ReadUntil('<');
                }
            }

            return element;
        }

        public void Parse()
        {
            _rootNode = TryParseNode();

            if (_rootNode == null)
                throw new XMLFragmentException("Unable to load root node");
        }
    }

    #endregion
}