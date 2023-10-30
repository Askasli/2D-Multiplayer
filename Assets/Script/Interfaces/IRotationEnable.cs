
    using System;

    public interface IRotationEnable
    {
        event Action<bool> OnRotationChange;
        bool CanRotate();
        void SetRotationValue(bool value);
    }
