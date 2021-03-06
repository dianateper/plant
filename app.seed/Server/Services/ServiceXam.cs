﻿using Models.Model;
using Server.Contracts;
using Server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    class ServiceXam : IContractXam
    {
        MachineRepository MachineRepository = new MachineRepository();
        PositionRepository PositionRepository = new PositionRepository();
        ControllerRepository ControllerRepository = new ControllerRepository();

        public List<Machine> GetAllMachines()
        {
            return MachineRepository.GetAllMachines();
        }

        public List<Position> GetAllPositions()
        {
            return PositionRepository.GetAllPosition();
        }

        public bool ChangeMachinePosition(Machine machine)
        {
            return MachineRepository.SetNewMachineLocation(machine);
        }

        public LinkedList<Position> GetOptimalRoute(Position start, Position end)
        {
            List<Position> positions = PositionRepository.GetAllPosition();
            
            Dijkstra.GraphBuilder graphBuilder = new Dijkstra.GraphBuilder();
            Dijkstra.Graph<Position> graph = SetGraph(positions, start, end);
            
            return graphBuilder.Search(start, end, graph);
        }

        Dijkstra.Graph<Position> SetGraph(List<Position> positions, Position start, Position end)
        {
            Dijkstra.Graph<Position> graph = new Dijkstra.Graph<Position>();
            List<Position> addedPositions = new List<Position>();

            graph.AddNode(start);
            addedPositions.Add(start);

            graph.AddNode(end);
            addedPositions.Add(end);

            positions.ForEach(i =>
            {
                if (i.X % 2 != 0 || i.Y % 2 != 0)
                {
                    graph.AddNode(i);
                    addedPositions.Add(i);
                }
            });

            addedPositions.ForEach(i =>
            {
                addedPositions.ForEach(j =>
                {
                    if (
                    (i.X == j.X + 1 && i.Y == j.Y) ||
                    (i.X == j.X - 1 && i.Y == j.Y) ||
                    (i.Y == j.Y + 1 && i.X == j.X) ||
                    (i.Y == j.Y - 1 && i.X == j.X)
                    )
                    {
                        graph.AddEdge(i, j, 1);
                    }
                });
            });

            return graph;
        }
        
        public Controller GetControllerByPosition(int X, int Y)
        {
            return ControllerRepository.GetControllerByXAndY(X, Y);
        }

        public List<Controller> GetAllControllers()
        {
            return ControllerRepository.GetAllControllersData();
        }
    }
}
