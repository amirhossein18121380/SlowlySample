namespace SlowlySimulate.Shared.Dto
{
    public interface IMementoDto
    {
        void SaveState();
        void RestoreState();
        void ClearState();
    }
}
