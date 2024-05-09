using NetworkingModule.common;

namespace SubaruImpreza
{
    public interface ResponseObserver
    {
        void handleResponeParticipant(IResponse response);
        void handleResponeUser(IResponse response);

    }
}