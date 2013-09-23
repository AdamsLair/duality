using System;
using System.Collections.Generic;
using System.Linq;
using Duality.Resources;

namespace Duality
{
	/// <summary>
	/// Defines priorities for the budget-based sound system
	/// </summary>
	public enum SoundBudgetPriority
	{
		/// <summary>
		/// Highest priority. Overpowers anything else. Value: 100.
		/// </summary>
		Highest		= 100,
		/// <summary>
		/// Second-highest priority, designated for action pads. Value: 80.
		/// </summary>
		Action		= 80,
		/// <summary>
		/// Priority of an alarming pad. Value: 60.
		/// </summary>
		Alarm		= 60,
		/// <summary>
		/// Priority of a pad to build up tension. Value: 40.
		/// </summary>
		Tension		= 40,
		/// <summary>
		/// Second-lowest priority, designated for background pads. Value: 20.
		/// </summary>
		Background	= 20,
		/// <summary>
		/// Lowest priority. Is overpowered by anything else. Value: 0.
		/// </summary>
		Lowest		= 0,

		/// <summary>
		/// Priority offset from one default priority to the next.
		/// </summary>
		StepOne		= 20,
		/// <summary>
		/// Priority offset for a half step in the default priority scale.
		/// </summary>
		StepHalf	= 10,
		/// <summary>
		/// Priority offset for a quarter step in the default priority scale.
		/// </summary>
		StepQuarter	= 5,
		/// <summary>
		/// Smalles possible priority offset in the default priority scale.
		/// </summary>
		StepMicro	= 1
	}

	/// <summary>
	/// Wraps a SoundInstance in order to make it budget-based. Intended
	/// only to be used for longer sound pads such as ambient or music.
	/// </summary>
	public sealed class SoundBudgetPad : IDisposable
	{
		private static int idCounter = 0;

		private	bool			disposed	= false;
		private	SoundInstance	instance	= null;
		private	SoundBudgetPriority	prio	= SoundBudgetPriority.Lowest;
		private	bool			giveUp		= false;
		private	bool			weak		= false;
		private	float			weight		= 1.0f;
		private	int				sortId		= ++idCounter;

		/// <summary>
		/// [GET] Whether the object has been disposed. Disposed objects are not to be used.
		/// Treat them as null or similar.
		/// </summary>
		public bool Disposed
		{
			get { return disposed; }
		}
		/// <summary>
		/// [GET] The <see cref="SoundInstance"/> that is wrapped by this budget pad.
		/// </summary>
		public SoundInstance Instance
		{
			get { return this.instance; }
		}		//	G
		/// <summary>
		/// [GET] The priority of this budget pad. Higher priorities overpower lower priorities.
		/// </summary>
		public SoundBudgetPriority Priority
		{
			get { return this.prio; }
		}	//	G
		/// <summary>
		/// [GET] The numeric value of this budget pads priority.
		/// </summary>
		public int PriorityValue
		{
			get { return (int)this.prio; }
		}				//	G
		/// <summary>
		/// [GET] The budget pads additional sorting id.
		/// </summary>
		public int SortId
		{
			get { return this.sortId; }
		}					//	G
		/// <summary>
		/// [GET / SET] Whether this budget pad disposes itsself if it doesn't get any budget, i.e.
		/// is overpowered by another budget pad and isn't audible anyway.
		/// </summary>
		public bool GivesUpWithoutBudget
		{
			get { return this.giveUp; }
			set { this.giveUp = value; }
		}		//	GS
		/// <summary>
		/// [GET / SET] Whether this is a weak budget pad. Weak pads are automatically disposed as
		/// soon as a non-weak pad overpowers them. This behaviour can for example to be used for 
		/// automatically playing background music that is to be removed as soon as there's a scripted
		/// sequence playing its own, specific music.
		/// </summary>
		public bool Weak
		{
			get { return this.weak; }
			set { this.weak = value; }
		}						//	GS
		/// <summary>
		/// [GET / SET] The budget weight this pad consumes.
		/// </summary>
		public float Weight
		{
			get { return this.weight; }
			set { this.weight = value; }
		}					//	GS
		
		internal SoundBudgetPad(SoundInstance snd, SoundBudgetPriority prio)
		{
			this.instance = snd;
			this.prio = prio;
		}
		~SoundBudgetPad()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool manually)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.OnDisposed(manually);
			}
		}
		private void OnDisposed(bool manually)
		{
			this.prio = SoundBudgetPriority.Lowest;
			if (this.instance != null)
			{
				this.instance.Stop();
				this.instance = null;
			}
		}

		/// <summary>
		/// Updates the budget pad.
		/// </summary>
		/// <param name="nonWeakAbove"></param>
		/// <param name="budget"></param>
		public void Update(bool nonWeakAbove, ref float budget)
		{
			if (this.instance == null || this.instance.Disposed)
			{
				this.Dispose();
				return;
			}

			float myBudget = Math.Min(this.weight, budget);
			float myFadedBudget = Math.Min(this.weight * this.instance.CurrentFade, budget);
			budget -= myFadedBudget;

			if (myBudget < 0.01f && (this.giveUp || (this.weak && nonWeakAbove)))
			{
				this.Dispose();
				return;
			}

			this.instance.Volume = myFadedBudget;
			this.instance.Paused = (myBudget <= 0.01f && this.instance.FadeTarget > 0.0f);
		}
	}

	/// <summary>
	/// A queue of <see cref="SoundBudgetPad">SoundBudgetPads</see>.
	/// </summary>
	public sealed class SoundBudgetQueue
	{
		/// <summary>
		/// A pads default fadein time in seconds.
		/// </summary>
		public const float DefaultFadeInTime = 3.0f;
		/// <summary>
		/// A pads default fadeout time in seconds.
		/// </summary>
		public const float DefaultFadeOutTime = 3.0f;

		private	List<SoundBudgetPad>	budgetPads		= new List<SoundBudgetPad>();
		
		/// <summary>
		/// Determines whether there is currently any pad scheduled.
		/// </summary>
		/// <returns>True, if there is, false if not.</returns>
		public bool IsAnyScheduled
		{
			get { return this.budgetPads.Any(pad => pad.Instance != null && pad.Instance.FadeTarget > 0.0f); }
		}
		/// <summary>
		/// [GET] Returns the main active pad, i.e. the one that is scheduled with the highest priority.
		/// </summary>
		public SoundBudgetPad ActiveMainPad
		{
			get { return this.budgetPads.Count > 0 ? this.budgetPads[this.budgetPads.Count - 1] : null; }
		}
		/// <summary>
		/// [GET] Enumerates all active pads, sorted descending by priority.
		/// </summary>
		public IEnumerable<SoundBudgetPad> ActivePads
		{
			get { return (this.budgetPads as IEnumerable<SoundBudgetPad>).Reverse(); }
		}
		/// <summary>
		/// [GET] Returns the main active sound, i.e. the one that is scheduled with the highest priority.
		/// </summary>
		public ContentRef<Sound> ActiveMainSound
		{
			get
			{
				SoundBudgetPad pad = this.ActiveMainPad;
				if (pad == null || pad.Instance == null) return null;
				return pad.Instance.Sound;
			}
		}
		/// <summary>
		/// [GET] Enumerates all active sounds, sorted descending by priority.
		/// </summary>
		public IEnumerable<ContentRef<Sound>> ActiveSounds
		{
			get
			{
				return this.ActivePads.Select(p => p.Instance != null ? p.Instance.Sound : null);
			}
		}

		/// <summary>
		/// Updates the queue.
		/// </summary>
		public void Update()
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Game)
				this.Clear(0.0f);

			float budget = 1.0f;
			bool nonWeakPassed = false;

			// Lowest priority at begin - we're travelling backwards
			this.budgetPads.StableSort(delegate(SoundBudgetPad obj1, SoundBudgetPad obj2)
				{
					int result = obj1.PriorityValue - obj2.PriorityValue;
					if (result != 0)	return result;
					else				return obj2.SortId - obj1.SortId;
				});

			for (int i = this.budgetPads.Count - 1; i >= 0; i--)
			{
				this.budgetPads[i].Update(nonWeakPassed, ref budget);
				if (!this.budgetPads[i].Weak) nonWeakPassed = true;
				if (this.budgetPads[i].Disposed) this.budgetPads.RemoveAt(i);
			}
		}
		
		/// <summary>
		/// Schedules a new budget pad.
		/// </summary>
		/// <param name="snd">The Sound that is to play.</param>
		/// <param name="priority">The priority of the pad.</param>
		/// <param name="fadeInTimeSec">The pads fadein time in seconds.</param>
		/// <returns>A new SoundBudgetPad.</returns>
		public SoundBudgetPad Push(ContentRef<Sound> snd, SoundBudgetPriority priority, float fadeInTimeSec = DefaultFadeInTime)
		{
			SoundInstance inst = DualityApp.Sound.PlaySound(snd);
			SoundBudgetPad bP = new SoundBudgetPad(inst, priority);
			bP.Instance.Paused = true;
			if (fadeInTimeSec > 0.05f) bP.Instance.BeginFadeIn(fadeInTimeSec);
			this.budgetPads.Add(bP);
			return bP;
		}
		/// <summary>
		/// Fades out all currently scheduled pads that use the specified <see cref="Duality.Resources.Sound"/>.
		/// </summary>
		/// <param name="snd"></param>
		/// <param name="fadeOutTimeSec"></param>
		public void Pop(ContentRef<Sound> snd, float fadeOutTimeSec = DefaultFadeOutTime)
		{
			foreach (SoundBudgetPad t in this.budgetPads)
			{
				if (t.Instance.Sound.Res == snd.Res)
				{
					t.Instance.FadeOut(fadeOutTimeSec);
				}
			}
		}

		/// <summary>
		/// Fades out the pad with the highest priority i.e. that is (most) audible.
		/// </summary>
		/// <param name="fadeOutTimeSec"></param>
		public void PopHigh(float fadeOutTimeSec = DefaultFadeOutTime)
		{
			if (this.budgetPads.Count == 0) return;
			this.budgetPads[this.budgetPads.Count - 1].Instance.FadeOut(fadeOutTimeSec);
		}
		/// <summary>
		/// Fades out the pad with the lowest priority i.e. that is (most) inaudible.
		/// </summary>
		/// <param name="fadeOutTimeSec"></param>
		public void PopLow(float fadeOutTimeSec = DefaultFadeOutTime)
		{
			if (this.budgetPads.Count == 0) return;
			this.budgetPads[0].Instance.FadeOut(fadeOutTimeSec);
		}
		/// <summary>
		/// Fades out all currently scheduled pads.
		/// </summary>
		/// <param name="fadeOutTimeSec"></param>
		public void Clear(float fadeOutTimeSec = DefaultFadeOutTime)
		{
			if (this.budgetPads.Count == 0) return;
			foreach (SoundBudgetPad t in this.budgetPads)
			{
				if (t.Instance != null)
				{
					t.Instance.FadeOut(fadeOutTimeSec);
				}
			}
		}
	}
}
