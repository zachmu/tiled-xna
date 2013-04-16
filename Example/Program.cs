using System;

namespace TiledExample {
    static class Program {
        static void Main (string[] args) {
            using (TiledExample game = new TiledExample()) {
                game.Run();
            }
        }
    }
}

