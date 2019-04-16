using System;
using System.Runtime.InteropServices;

namespace SierraLib.Win32API
{
    public static class SystemPowerState
    {
        [Flags]
        public enum ExecutionState : uint
        {
            /// <summary>
            /// Prevents the system from entering sleep/low power mode.
            /// </summary>
            SystemRequired = 0x1,

            /// <summary>
            /// Prevents the display from entering sleep/low power mode.
            /// </summary>
            DisplayRequired = 0x2,

            /// <summary>
            /// Notifies the OS that the current application requires
            /// the system to continue operation while imitating a
            /// sleep/low power mode.
            /// </summary>
            AwayModeRequired = 0x40,

            /// <summary>
            /// Resets any present flags if used alone or enables the selected
            /// flag until <see cref="Reset"/> is called.
            /// </summary>
            Continuous = 0x80000000,

            UserPresent = 0x4
        }

        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(ExecutionState executionFlags);

        public static bool PreventStandby()
        {
            return SetExecutionState(ExecutionState.SystemRequired | ExecutionState.Continuous);
        }

        public static bool Reset()
        {
            return SetExecutionState(ExecutionState.Continuous);
        }

        public static bool PreventScreenStandby()
        {
            return SetExecutionState(ExecutionState.DisplayRequired | ExecutionState.Continuous);
               
        }

        public static bool SetExecutionState(ExecutionState flags)
        {
            uint res = SetThreadExecutionState(flags);

            if (res == 0)
                return false;
            return true;

        }
    }
}
