using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Services
{
    public interface IGameService
    {
        Matrix Projection { get; }

        Matrix View { get; set; }

        Vector3 PlayerPosition { get; set; }

        float Scale { get; set; }
    }
}
