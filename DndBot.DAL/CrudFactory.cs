using DndBot.Entities;
using MongoORM4NetCore;
using MongoORM4NetCore.Interfaces;
using MongoORM4NetCore.Structs;
using System;
using System.Collections.Generic;

namespace DndBot.DAL
{
    public static class CrudFactory
    {
        public static void OpenConnection(string databaseName = "", string serverIP = "localhost", int port = 27017, string userName = "", string password = "",
            Dictionary<string, string> connectionStringOptions = null,
            IEnumerable<MongoConnectionStringReplicas> connectionStringReplicas = null)
        {
            MongoDbConnection.InitializeAndStartConnection(databaseName, serverIP, port, userName, password, connectionStringOptions, connectionStringReplicas);
        }
    }
}
