namespace SubaruImpreza
{
    public interface Subject
    {
        void addObserver(ResponseObserver observer);
        void notifyObserver();
    }
}