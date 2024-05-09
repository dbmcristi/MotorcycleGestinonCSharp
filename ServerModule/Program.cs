using ServerModule.controller;

namespace ServerModule
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SocketController controller = new SocketController();
            controller.Start();
        }
    }
}