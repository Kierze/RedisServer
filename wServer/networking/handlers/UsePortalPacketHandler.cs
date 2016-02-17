using System;
using System.Globalization;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.worlds;

namespace wServer.networking.handlers
{
    internal class UsePortalPacketHandler : PacketHandlerBase<UsePortalPacket>
    {
        public override PacketID ID { get { return PacketID.USEPORTAL; } }

        protected override void HandlePacket(Client client, UsePortalPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                var portal = client.Player.Owner.GetEntity(packet.ObjectId) as Portal;
                if (portal == null) return;

                if (!portal.Usable)
                {
                    client.Player.SendError("{\"key\":\"server.realm_full\"}");
                    return;
                }

                var world = portal.WorldInstance;

                if (world == null)
                {
                    bool setWorldInstance = true;
                    var desc = portal.ObjectDesc;
                    if (desc == null)
                        SendFailure(client, "Portal not found");
                    else
                    {
                        switch (portal.ObjectType)
                        {
                            case 0x0720:
                                world = client.Player.Manager.GetWorld(World.VAULT_ID);
                                setWorldInstance = false;
                                break;

                            case 0x0704:
                            case 0x0703:
                            case 0x0d40:
                            case 0x070d:
                            case 0x070e:
                                {
                                    if (client.Player.Manager.PlayerWorldMapping.ContainsKey(client.Player.AccountId))
                                    {
                                        var w = client.Player.Manager.PlayerWorldMapping[client.Player.AccountId];
                                        if (w != null && client.Player.Manager.Worlds.ContainsKey(w.Id))
                                            world = w;
                                        else
                                            world = client.Player.Manager.GetWorld(World.NEXUS_ID);
                                    }
                                    else
                                        world = client.Player.Manager.GetWorld(World.NEXUS_ID);
                                    setWorldInstance = false;
                                }
                                break;

                            //case 0x0750:
                            //    world = client.Player.Manager.GetWorld(World.MARKET);
                            //    break;

                            case 0x071d:
                                world = client.Player.Manager.GetWorld(World.NEXUS_ID);
                                break;

                            //case 0x0753:
                            //    world = client.Manager.AddWorld(new PetYard(client.Player));
                            //    setWorldInstance = false;
                            //    break;

                            case 0x0712:
                                world = client.Player.Manager.GetWorld(World.NEXUS_ID);
                                break;

                            //case 0x1756:
                            //    world = client.Player.Manager.GetWorld(World.DAILY_QUEST_ID);
                            //    break;

                            //case 0x072f:
                            //    if (client.Player.Guild != null)
                            //    {
                            //        //client.Player.SendInfo(
                            //        //    "Sorry, you are unable to enter the GuildHall because of a possible memory leak, check back later");
                            //        //client.Player.SendInfo("Thanks.");
                            //        world = client.Player.Guild.GuildHall;
                            //    }
                            //    break;

                            default:
                                var worldType =
                                    Type.GetType("wServer.realm.worlds." +
                                                 desc.DungeonName.Replace(" ", String.Empty).Replace("'", String.Empty));
                                if (worldType != null)
                                {
                                    try
                                    {
                                        world = client.Manager.AddWorld((World)Activator.CreateInstance(worldType,
                                            System.Reflection.BindingFlags.CreateInstance, null, null,
                                            CultureInfo.InvariantCulture, null));
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                else
                                    client.Player.SendError("WorldClass for " + desc.DungeonName +
                                                            " not found, can not load world.");
                                break;
                        }
                        if (setWorldInstance)
                            portal.WorldInstance = world;
                    }

                    if (world != null)
                    {
                        if (client.Player.Manager.PlayerWorldMapping.ContainsKey(client.Player.AccountId))
                        {
                            World dummy;
                            client.Player.Manager.PlayerWorldMapping.TryRemove(client.Player.AccountId, out dummy);
                        }
                        if (client.Player.Owner is Nexus || client.Player.Owner is GameWorld)
                            client.Player.Manager.PlayerWorldMapping.TryAdd(client.Player.AccountId, client.Player.Owner);

                        client.Reconnect(new ReconnectPacket
                        {
                            Host = "",
                            Port = Program.Settings.GetValue<int>("port"),
                            GameId = world.Id,
                            Name = world.Name,
                            Key = Empty<byte>.Array,
                        });
                    }
                }
            });
        }
    }
}
