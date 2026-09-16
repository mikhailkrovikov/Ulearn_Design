namespace Memory.API;

public class APIObject : IDisposable
{
    private bool isDisposed = false;
    private readonly int id;
    public APIObject(int id)
    {
        this.id = id;
        MagicAPI.Allocate(id);
    }

    ~APIObject()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            MagicAPI.Free(id);
            isDisposed = true;
        }
    }
}