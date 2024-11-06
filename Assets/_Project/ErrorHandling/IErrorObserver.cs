namespace Metaverse.ErrorHandling
{
    internal interface IErrorObserver
    {
        void HandleObservation(Error error);
    }
}