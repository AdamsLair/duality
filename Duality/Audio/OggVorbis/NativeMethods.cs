using System.Runtime.InteropServices;

namespace Duality.OggVorbis
{
	public static class NativeMethods
	{
		public enum ErrorCode
		{
			None = 0,
			OpenFailed = 1,				// open file failed
			MallocFailed = 2,			// malloc() call failed; out of memory
			ReadFailed = 3,				// A read from media returned an error     
			NotVorbisData = 4,			// Bitstream is not Vorbis data       
			VorbisVersionMismatch = 5,	// Vorbis version mismatch
			InvalidVorbisHeader = 6,	// Invalid Vorbis bitstream header
			InternalFault = 7,			// Internal logic fault; indicates a bug or heap/stack corruption
			UnspecifiedError = 8		// ov_open() returned an undocumented error
		}

		// Initialization for decoding the given Ogg Vorbis file name.
		[DllImport("OggvorbisDotNet.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int init_for_ogg_decode(
			string fileName, void** vf_out);

		// Initialization for decoding the given Ogg Vorbis memory stream.
		[DllImport("OggvorbisDotNet.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern int memory_stream_for_ogg_decode(
			byte[] stream, int sizeOfStream, void** vf_out);

		// Writes Pulse Code Modulation (PCM) data into the given buffer beginning 
		// at buf_out[0].
        [DllImport("OggvorbisDotNet.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int ogg_decode_one_vorbis_packet(
            void* vf_ptr, void* buf_out, int buf_byte_size, 
			int bits_per_sample, int* channels_cnt, int* sampling_rate, 
			int* err_ov_hole_cnt, int* err_ov_ebadlink_cnt);

		//	Free the memory pointed to by vf_out and also close the Ogg Vorbis file 
		//	opened by init_for_ogg_decode(). OK to call with null vf_ptr parameter.
        [DllImport("OggvorbisDotNet.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int final_ogg_cleanup(void* vf_ptr);
	}
}
