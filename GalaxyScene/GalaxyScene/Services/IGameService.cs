using GalaxyScene.Model;
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

        Player Player { get; set; }

        float Scale { get; set; }
    }
}
