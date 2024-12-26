#if PROTOBUF_V3
package Game.Protobuf.Types;
import "google/protobuf/timestamp.proto";
/// <summary>
/// Client <=> FightingRoomServer Protobuf Message
/// </summary>

enum FightingRoomPlayerPosition
{
  No_Position    = 0; // 不在房內
  Reject        = 1; // 無法進入
  Player_1P      = 2;
  Player_2P      = 3;
  Bench         = 4;
}

enum FightingRoomSyncType
{
  Lockstep      = 0; // 鎖幀, 配合DelayFrame和Rollback使遊玩體感順暢
  Occ           = 1; // 樂觀並行控制(Optimistic Concurrency Control)
}

// RoomAction:BattleGameState SyncId
enum FightingRoomBattleGameState
{
  Undefined         = 0;
  Prepare           = 1;
  Game_Start        = 2;
  Battle_Start      = 3;
  Round_Start       = 4;
  Round_End         = 5;
}

enum BattleResult
{
    ResultError     = 0;
    P1Win           = 1;
    P2Win           = 2;
    Draw            = 3;
    Timeout         = 4;
}

message CharacterData
{
  uint64 Character_Id    = 1;
  string Show_Name       = 2;
  uint32 Character_Flag  = 3;
}

message RegisterCharacterData
{
  RequestJoinFightingRoom Join_Room_Ticket = 1;
  CharacterData Character_Data = 2;
}

message FightingRoomCharacterStateData
{
  uint64 Character_Id                               =  1;   // 角色 id
  string Character_Name                             =  2;   // 角色名稱
  int32 Role_Id                                     =  3;   // 角色 id
  int32 Weapon_Id                                   =  4;   // 武器 id
  int32 Sub_Weapon_Id                               =  5;   // 副武器 id
  int32 Head_Id                                     =  6;   // 頭飾紙娃娃 id
  int32 Cloth_Id                                    =  7;   // 衣服紙娃娃 id
  int32 Backpack_Id                                 =  8;   // 背包紙娃娃 id
  int32 Hp                                          =  9;   // 血量
  int32 Atk                                         =  10;   // 攻擊力
  int32 Walk_Speed                                  =  11;   // 移動速度
  int32 Same_Move_Correction                        =  12;   // 同招式補正
  repeated int32 Combo_Correction                   =  13;  // combo傷害補正
  repeated int32 Air_Combo_Correction               =  14;  // 空中combo傷害補正
  int32 Is_Ready_Play_Again                         =  15;
  int32 Is_Ready                                    =  16;
  FightingRoomPlayerPosition Position               =  17;
  FightingRoomBattleGameState Battle_Game_State     =  18;
}

message FightingRoomBattleSetting
{
  int32 Bo = 1;
  int32 Round_Time = 2;
  int32 Input_Delay_Frame = 3;
  int32 Input_Fps = 4;
  repeated int64 Round_Random_Seeds = 5;
}

message FightingRoomSetting
{
    int32 Error_Code = 1;
    int32 Stage_Id = 2;
    FightingRoomSyncType Sync_Type = 3;
    repeated FightingRoomCharacterStateData Characters_Info = 4;
    FightingRoomBattleSetting Battle_Setting = 5;
}

message FightingRoundData
{
  int32 Winner_Character_Id = 1;
}

message FightingRoomStageStateData
{
  int32 Is_Planning                              = 1;
  int32 Stage_Id                                 = 2;
  int32 Current_Round                            = 3;
  repeated FightingRoundData Round_Data          = 4;
  FightingRoomBattleSetting Battle_Setting       = 5;
  FightingRoomBattleGameState Battle_Game_State  = 6;
}


// ==========PING_PONG==========
message FightingRoomPingData
{
  int32 Id = 1;
  google.protobuf.Timestamp Ping_Time = 2;
  google.protobuf.Timestamp Pong_Time = 3;
}



// ==========NET_USER_SYNC==========
message SignalCharacterSync
{
  uint32 Sync_Flag = 1;
  string Show_Name = 2;
}
message CommandCharacterSync
{
  int32 Is_You                       = 1;
  CharacterData Character_Data       = 2;
}
// ==============================



// ==========CREATE_ROOM==========
message RequestCreateFightingRoom
{
  string Room_Key                            = 1;
  FightingRoomSyncType Room_Sync_Type         = 2;
  FightingRoomBattleSetting Battle_Setting   = 3;
}

// ==============================



// ==========JOIN_ROOM==========
message RequestJoinFightingRoom
{
  string Room_Id     = 1;
  string Room_Key    = 2;
}

// ==============================



// ==========LEAVE_ROOM==========
message RequestLeaveFightingRoom
{
}
// ==============================



// ==========ROOM_SYNC==========
message CommandFightingRoomSync
{
  string Room_Id                                                    = 1;
  uint64 Room_Master_Character_Id                                   = 2;
  FightingRoomSyncType Room_Sync_Type                               = 3;    // 同步類型 鎖幀或OCC
  FightingRoomStageStateData Stage_State_Data                       = 4;
  repeated FightingRoomCharacterStateData Character_State_Datas     = 5;
}

// ==============================



// ==========ROOM_ACTION==========
// Server To Client
message CommandFightingRoomAction
{
  enum CommandActionType
  {
    Undefined                                = 0;
	Room_Enter                               = 1;
	Room_Leave                               = 2;
    Plan_Ready_On                            = 3;
    Plan_Ready_Off                           = 4;
    Role_Id                                  = 6;  // use sync_int_id
    Weapon_Id                                = 7;  // use sync_int_id
    Stage_Id                                 = 8;  // use sync_int_id
    Change_Position                          = 9;  // use sync_position
    Goto_Battle_Stage                        = 10;
    Play_Again_Type                          = 11; // use sync_int_id
    Skip_Animation                           = 12;
  }
  CommandActionType Action_Type              = 1;
  uint64 Character_Id                        = 2;
  int32 Sync_Int_Id                          = 3;
  FightingRoomPlayerPosition Sync_Position   = 4;
}

// Client To Server
message SignalFightingRoomAction
{
  enum SignalActionType
  {
    Undefined                               = 0;
    Plan_Ready_On                           = 1;
    Plan_Ready_Off                          = 2;
    Plan_Start_Game                         = 3;
    Role_Id                                 = 4;    // use sync_int_id
    Weapon_Id                               = 5;    // use sync_int_id
    Stage_Id                                = 6;    // use sync_int_id
    Change_Position                         = 7;    // use sync_position
    Set_Input_Delay_Frame                   = 8;    // use sync_int_id
    Battle_Game_State                       = 9;    // use sync_int_id
    Set_Game_Bo                             = 10;   // use sync_int_id
    Set_Round_Time                          = 11;   // use sync_int_id
    Set_Input_Fps                           = 12;   // use sync_int_id
    Play_Again_Type                         = 13;   // use sync_int_id
    Skip_Animation                          = 14;
  }
  SignalActionType Action_Type              = 1;
  int32 Sync_Int_Id                         = 2;
  FightingRoomPlayerPosition Sync_Position  = 3;
}
// ==============================

// ==========BATTLE_KEY_DOWN_SYNC==========
message FightingRooomBattleKeyDownSync
{
  int32 Round = 1;
  int32 Player_Id = 2;
  int32 Flag_From_Frame = 3;
  repeated uint32 Key_Down_Flag = 4;
  int32 Last_Receive_Advance_Frame_X100 = 5;	// 最近一次收到指令時, 續離執行該指令還差多少Frame(+早到, -晚到)
}
// ==============================

// ==========SEND_BATTLE_RESULT==========
message BattleActionDataLog
{
    int32           RoleId                      = 1;
    int32           WeaponId                    = 2;
    int32           SubWeaponId                 = 3;
    int32           WeaponBrokenTimes           = 4;
    int32           SubWeaponBrokenTimes        = 5;
    int32           UsedThrowTimes              = 6;
    int32           UsedDeThrowTimes            = 7;
    int32           UsedReversalTimes           = 8;
    uint32          CharacterId                 = 9;
}

message FightingRoomBattleResult
{
    int32           ErrorCode                   = 1;
    string          TicketPasscode              = 2;
    BattleResult    Result                      = 3;
    int32           IsSuddenDeath               = 4;
    repeated BattleActionDataLog BattleAction   = 5;
}


// ==========SEND_BATTLE_REPORT==========

message ReportFrameData
{
    int32       Frame               = 1;
    string      Key                 = 2;
    string      Location            = 3;
    string      Rotation            = 4;
    int32       Hp                  = 5;
    int32       State               = 6;
    int32       Move                = 7;
    int32       Animation           = 8;
    string      KeyEventArray       = 9;
    string      KeyPoolArray        = 10;
    string      Event               = 11;
}

message FightingPlayerReport
{
    int32       RoleId                      = 1;
    int32       WeaponId                    = 2;
    int32       SubWeaponId                 = 3;
    repeated ReportFrameData FrameAction    = 4;
}

message RoundPlayerReportData
{
    int32    Round                           = 1;
    repeated FightingPlayerReport PlayerData = 2;
}

message FightingRoomReport
{
    repeated string Battle_Reports = 1;
}

message ProtoVector
{
    int32   X                   = 1;
    int32   Y                   = 2;
    int32   Z                   = 3;
}

message ProtoRotate
{
    int32   Pitch               = 1;
    int32   Yaw                 = 2;
    int32   Roll                = 3;
}

message BattleReportStateData
{              
    ProtoVector Vactor           = 1;
    ProtoRotate Rotate           = 2;
    int32   Hp                   = 3;
    int32   FighterStateCtrlID   = 4;
    uint32  KeyDownFlag          = 5;
    uint32  KeyDownFlagLast      = 6;
    int32   AminID               = 7;
    int32   MoveID               = 8;
}

message BattleReportPlayerData
{
    int32   SelfPlayerId                                = 1;
    int32   Round                                       = 2;
    int32   Frame                                       = 3;
    repeated BattleReportStateData PlayerStateData      = 4;
}

#endif  // PROTOBUF_V3

// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: FightingRoomProto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using global::Google.Protobuf.Compatibility;
using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Game.Protobuf.Types {

  /// <summary>Holder for reflection information generated from FightingRoomProto.proto</summary>
  public static partial class FightingRoomProtoReflection {

    #region Descriptor
    /// <summary>File descriptor for FightingRoomProto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static FightingRoomProtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdGaWdodGluZ1Jvb21Qcm90by5wcm90bxITR2FtZS5Qcm90b2J1Zi5UeXBl",
            "cxofZ29vZ2xlL3Byb3RvYnVmL3RpbWVzdGFtcC5wcm90byJQCg1DaGFyYWN0",
            "ZXJEYXRhEhQKDENoYXJhY3Rlcl9JZBgBIAEoBBIRCglTaG93X05hbWUYAiAB",
            "KAkSFgoOQ2hhcmFjdGVyX0ZsYWcYAyABKA0imwEKFVJlZ2lzdGVyQ2hhcmFj",
            "dGVyRGF0YRJGChBKb2luX1Jvb21fVGlja2V0GAEgASgLMiwuR2FtZS5Qcm90",
            "b2J1Zi5UeXBlcy5SZXF1ZXN0Sm9pbkZpZ2h0aW5nUm9vbRI6Cg5DaGFyYWN0",
            "ZXJfRGF0YRgCIAEoCzIiLkdhbWUuUHJvdG9idWYuVHlwZXMuQ2hhcmFjdGVy",
            "RGF0YSKDBAoeRmlnaHRpbmdSb29tQ2hhcmFjdGVyU3RhdGVEYXRhEhQKDENo",
            "YXJhY3Rlcl9JZBgBIAEoBBIWCg5DaGFyYWN0ZXJfTmFtZRgCIAEoCRIPCgdS",
            "b2xlX0lkGAMgASgFEhEKCVdlYXBvbl9JZBgEIAEoBRIVCg1TdWJfV2VhcG9u",
            "X0lkGAUgASgFEg8KB0hlYWRfSWQYBiABKAUSEAoIQ2xvdGhfSWQYByABKAUS",
            "EwoLQmFja3BhY2tfSWQYCCABKAUSCgoCSHAYCSABKAUSCwoDQXRrGAogASgF",
            "EhIKCldhbGtfU3BlZWQYCyABKAUSHAoUU2FtZV9Nb3ZlX0NvcnJlY3Rpb24Y",
            "DCABKAUSGAoQQ29tYm9fQ29ycmVjdGlvbhgNIAMoBRIcChRBaXJfQ29tYm9f",
            "Q29ycmVjdGlvbhgOIAMoBRIbChNJc19SZWFkeV9QbGF5X0FnYWluGA8gASgF",
            "EhAKCElzX1JlYWR5GBAgASgFEkEKCFBvc2l0aW9uGBEgASgOMi8uR2FtZS5Q",
            "cm90b2J1Zi5UeXBlcy5GaWdodGluZ1Jvb21QbGF5ZXJQb3NpdGlvbhJLChFC",
            "YXR0bGVfR2FtZV9TdGF0ZRgSIAEoDjIwLkdhbWUuUHJvdG9idWYuVHlwZXMu",
            "RmlnaHRpbmdSb29tQmF0dGxlR2FtZVN0YXRlIoUBChlGaWdodGluZ1Jvb21C",
            "YXR0bGVTZXR0aW5nEgoKAkJvGAEgASgFEhIKClJvdW5kX1RpbWUYAiABKAUS",
            "GQoRSW5wdXRfRGVsYXlfRnJhbWUYAyABKAUSEQoJSW5wdXRfRnBzGAQgASgF",
            "EhoKElJvdW5kX1JhbmRvbV9TZWVkcxgFIAMoAyKPAgoTRmlnaHRpbmdSb29t",
            "U2V0dGluZxISCgpFcnJvcl9Db2RlGAEgASgFEhAKCFN0YWdlX0lkGAIgASgF",
            "EjwKCVN5bmNfVHlwZRgDIAEoDjIpLkdhbWUuUHJvdG9idWYuVHlwZXMuRmln",
            "aHRpbmdSb29tU3luY1R5cGUSTAoPQ2hhcmFjdGVyc19JbmZvGAQgAygLMjMu",
            "R2FtZS5Qcm90b2J1Zi5UeXBlcy5GaWdodGluZ1Jvb21DaGFyYWN0ZXJTdGF0",
            "ZURhdGESRgoOQmF0dGxlX1NldHRpbmcYBSABKAsyLi5HYW1lLlByb3RvYnVm",
            "LlR5cGVzLkZpZ2h0aW5nUm9vbUJhdHRsZVNldHRpbmciMAoRRmlnaHRpbmdS",
            "b3VuZERhdGESGwoTV2lubmVyX0NoYXJhY3Rlcl9JZBgBIAEoBSKrAgoaRmln",
            "aHRpbmdSb29tU3RhZ2VTdGF0ZURhdGESEwoLSXNfUGxhbm5pbmcYASABKAUS",
            "EAoIU3RhZ2VfSWQYAiABKAUSFQoNQ3VycmVudF9Sb3VuZBgDIAEoBRI6CgpS",
            "b3VuZF9EYXRhGAQgAygLMiYuR2FtZS5Qcm90b2J1Zi5UeXBlcy5GaWdodGlu",
            "Z1JvdW5kRGF0YRJGCg5CYXR0bGVfU2V0dGluZxgFIAEoCzIuLkdhbWUuUHJv",
            "dG9idWYuVHlwZXMuRmlnaHRpbmdSb29tQmF0dGxlU2V0dGluZxJLChFCYXR0",
            "bGVfR2FtZV9TdGF0ZRgGIAEoDjIwLkdhbWUuUHJvdG9idWYuVHlwZXMuRmln",
            "aHRpbmdSb29tQmF0dGxlR2FtZVN0YXRlIoABChRGaWdodGluZ1Jvb21QaW5n",
            "RGF0YRIKCgJJZBgBIAEoBRItCglQaW5nX1RpbWUYAiABKAsyGi5nb29nbGUu",
            "cHJvdG9idWYuVGltZXN0YW1wEi0KCVBvbmdfVGltZRgDIAEoCzIaLmdvb2ds",
            "ZS5wcm90b2J1Zi5UaW1lc3RhbXAiOwoTU2lnbmFsQ2hhcmFjdGVyU3luYxIR",
            "CglTeW5jX0ZsYWcYASABKA0SEQoJU2hvd19OYW1lGAIgASgJImIKFENvbW1h",
            "bmRDaGFyYWN0ZXJTeW5jEg4KBklzX1lvdRgBIAEoBRI6Cg5DaGFyYWN0ZXJf",
            "RGF0YRgCIAEoCzIiLkdhbWUuUHJvdG9idWYuVHlwZXMuQ2hhcmFjdGVyRGF0",
            "YSK4AQoZUmVxdWVzdENyZWF0ZUZpZ2h0aW5nUm9vbRIQCghSb29tX0tleRgB",
            "IAEoCRJBCg5Sb29tX1N5bmNfVHlwZRgCIAEoDjIpLkdhbWUuUHJvdG9idWYu",
            "VHlwZXMuRmlnaHRpbmdSb29tU3luY1R5cGUSRgoOQmF0dGxlX1NldHRpbmcY",
            "AyABKAsyLi5HYW1lLlByb3RvYnVmLlR5cGVzLkZpZ2h0aW5nUm9vbUJhdHRs",
            "ZVNldHRpbmciPAoXUmVxdWVzdEpvaW5GaWdodGluZ1Jvb20SDwoHUm9vbV9J",
            "ZBgBIAEoCRIQCghSb29tX0tleRgCIAEoCSIaChhSZXF1ZXN0TGVhdmVGaWdo",
            "dGluZ1Jvb20irgIKF0NvbW1hbmRGaWdodGluZ1Jvb21TeW5jEg8KB1Jvb21f",
            "SWQYASABKAkSIAoYUm9vbV9NYXN0ZXJfQ2hhcmFjdGVyX0lkGAIgASgEEkEK",
            "DlJvb21fU3luY19UeXBlGAMgASgOMikuR2FtZS5Qcm90b2J1Zi5UeXBlcy5G",
            "aWdodGluZ1Jvb21TeW5jVHlwZRJJChBTdGFnZV9TdGF0ZV9EYXRhGAQgASgL",
            "Mi8uR2FtZS5Qcm90b2J1Zi5UeXBlcy5GaWdodGluZ1Jvb21TdGFnZVN0YXRl",
            "RGF0YRJSChVDaGFyYWN0ZXJfU3RhdGVfRGF0YXMYBSADKAsyMy5HYW1lLlBy",
            "b3RvYnVmLlR5cGVzLkZpZ2h0aW5nUm9vbUNoYXJhY3RlclN0YXRlRGF0YSLQ",
            "AwoZQ29tbWFuZEZpZ2h0aW5nUm9vbUFjdGlvbhJVCgtBY3Rpb25fVHlwZRgB",
            "IAEoDjJALkdhbWUuUHJvdG9idWYuVHlwZXMuQ29tbWFuZEZpZ2h0aW5nUm9v",
            "bUFjdGlvbi5Db21tYW5kQWN0aW9uVHlwZRIUCgxDaGFyYWN0ZXJfSWQYAiAB",
            "KAQSEwoLU3luY19JbnRfSWQYAyABKAUSRgoNU3luY19Qb3NpdGlvbhgEIAEo",
            "DjIvLkdhbWUuUHJvdG9idWYuVHlwZXMuRmlnaHRpbmdSb29tUGxheWVyUG9z",
            "aXRpb24i6AEKEUNvbW1hbmRBY3Rpb25UeXBlEg0KCVVuZGVmaW5lZBAAEg4K",
            "ClJvb21fRW50ZXIQARIOCgpSb29tX0xlYXZlEAISEQoNUGxhbl9SZWFkeV9P",
            "bhADEhIKDlBsYW5fUmVhZHlfT2ZmEAQSCwoHUm9sZV9JZBAGEg0KCVdlYXBv",
            "bl9JZBAHEgwKCFN0YWdlX0lkEAgSEwoPQ2hhbmdlX1Bvc2l0aW9uEAkSFQoR",
            "R290b19CYXR0bGVfU3RhZ2UQChITCg9QbGF5X0FnYWluX1R5cGUQCxISCg5T",
            "a2lwX0FuaW1hdGlvbhAMIv4DChhTaWduYWxGaWdodGluZ1Jvb21BY3Rpb24S",
            "UwoLQWN0aW9uX1R5cGUYASABKA4yPi5HYW1lLlByb3RvYnVmLlR5cGVzLlNp",
            "Z25hbEZpZ2h0aW5nUm9vbUFjdGlvbi5TaWduYWxBY3Rpb25UeXBlEhMKC1N5",
            "bmNfSW50X0lkGAIgASgFEkYKDVN5bmNfUG9zaXRpb24YAyABKA4yLy5HYW1l",
            "LlByb3RvYnVmLlR5cGVzLkZpZ2h0aW5nUm9vbVBsYXllclBvc2l0aW9uIq8C",
            "ChBTaWduYWxBY3Rpb25UeXBlEg0KCVVuZGVmaW5lZBAAEhEKDVBsYW5fUmVh",
            "ZHlfT24QARISCg5QbGFuX1JlYWR5X09mZhACEhMKD1BsYW5fU3RhcnRfR2Ft",
            "ZRADEgsKB1JvbGVfSWQQBBINCglXZWFwb25fSWQQBRIMCghTdGFnZV9JZBAG",
            "EhMKD0NoYW5nZV9Qb3NpdGlvbhAHEhkKFVNldF9JbnB1dF9EZWxheV9GcmFt",
            "ZRAIEhUKEUJhdHRsZV9HYW1lX1N0YXRlEAkSDwoLU2V0X0dhbWVfQm8QChIS",
            "Cg5TZXRfUm91bmRfVGltZRALEhEKDVNldF9JbnB1dF9GcHMQDBITCg9QbGF5",
            "X0FnYWluX1R5cGUQDRISCg5Ta2lwX0FuaW1hdGlvbhAOIpsBCh5GaWdodGlu",
            "Z1Jvb29tQmF0dGxlS2V5RG93blN5bmMSDQoFUm91bmQYASABKAUSEQoJUGxh",
            "eWVyX0lkGAIgASgFEhcKD0ZsYWdfRnJvbV9GcmFtZRgDIAEoBRIVCg1LZXlf",
            "RG93bl9GbGFnGAQgAygNEicKH0xhc3RfUmVjZWl2ZV9BZHZhbmNlX0ZyYW1l",
            "X1gxMDAYBSABKAUi5wEKE0JhdHRsZUFjdGlvbkRhdGFMb2cSDgoGUm9sZUlk",
            "GAEgASgFEhAKCFdlYXBvbklkGAIgASgFEhMKC1N1YldlYXBvbklkGAMgASgF",
            "EhkKEVdlYXBvbkJyb2tlblRpbWVzGAQgASgFEhwKFFN1YldlYXBvbkJyb2tl",
            "blRpbWVzGAUgASgFEhYKDlVzZWRUaHJvd1RpbWVzGAYgASgFEhgKEFVzZWRE",
            "ZVRocm93VGltZXMYByABKAUSGQoRVXNlZFJldmVyc2FsVGltZXMYCCABKAUS",
            "EwoLQ2hhcmFjdGVySWQYCSABKA0izwEKGEZpZ2h0aW5nUm9vbUJhdHRsZVJl",
            "c3VsdBIRCglFcnJvckNvZGUYASABKAUSFgoOVGlja2V0UGFzc2NvZGUYAiAB",
            "KAkSMQoGUmVzdWx0GAMgASgOMiEuR2FtZS5Qcm90b2J1Zi5UeXBlcy5CYXR0",
            "bGVSZXN1bHQSFQoNSXNTdWRkZW5EZWF0aBgEIAEoBRI+CgxCYXR0bGVBY3Rp",
            "b24YBSADKAsyKC5HYW1lLlByb3RvYnVmLlR5cGVzLkJhdHRsZUFjdGlvbkRh",
            "dGFMb2ciyQEKD1JlcG9ydEZyYW1lRGF0YRINCgVGcmFtZRgBIAEoBRILCgNL",
            "ZXkYAiABKAkSEAoITG9jYXRpb24YAyABKAkSEAoIUm90YXRpb24YBCABKAkS",
            "CgoCSHAYBSABKAUSDQoFU3RhdGUYBiABKAUSDAoETW92ZRgHIAEoBRIRCglB",
            "bmltYXRpb24YCCABKAUSFQoNS2V5RXZlbnRBcnJheRgJIAEoCRIUCgxLZXlQ",
            "b29sQXJyYXkYCiABKAkSDQoFRXZlbnQYCyABKAkiiAEKFEZpZ2h0aW5nUGxh",
            "eWVyUmVwb3J0Eg4KBlJvbGVJZBgBIAEoBRIQCghXZWFwb25JZBgCIAEoBRIT",
            "CgtTdWJXZWFwb25JZBgDIAEoBRI5CgtGcmFtZUFjdGlvbhgEIAMoCzIkLkdh",
            "bWUuUHJvdG9idWYuVHlwZXMuUmVwb3J0RnJhbWVEYXRhImUKFVJvdW5kUGxh",
            "eWVyUmVwb3J0RGF0YRINCgVSb3VuZBgBIAEoBRI9CgpQbGF5ZXJEYXRhGAIg",
            "AygLMikuR2FtZS5Qcm90b2J1Zi5UeXBlcy5GaWdodGluZ1BsYXllclJlcG9y",
            "dCIsChJGaWdodGluZ1Jvb21SZXBvcnQSFgoOQmF0dGxlX1JlcG9ydHMYASAD",
            "KAkiLgoLUHJvdG9WZWN0b3ISCQoBWBgBIAEoBRIJCgFZGAIgASgFEgkKAVoY",
            "AyABKAUiNwoLUHJvdG9Sb3RhdGUSDQoFUGl0Y2gYASABKAUSCwoDWWF3GAIg",
            "ASgFEgwKBFJvbGwYAyABKAUi8QEKFUJhdHRsZVJlcG9ydFN0YXRlRGF0YRIw",
            "CgZWYWN0b3IYASABKAsyIC5HYW1lLlByb3RvYnVmLlR5cGVzLlByb3RvVmVj",
            "dG9yEjAKBlJvdGF0ZRgCIAEoCzIgLkdhbWUuUHJvdG9idWYuVHlwZXMuUHJv",
            "dG9Sb3RhdGUSCgoCSHAYAyABKAUSGgoSRmlnaHRlclN0YXRlQ3RybElEGAQg",
            "ASgFEhMKC0tleURvd25GbGFnGAUgASgNEhcKD0tleURvd25GbGFnTGFzdBgG",
            "IAEoDRIOCgZBbWluSUQYByABKAUSDgoGTW92ZUlEGAggASgFIpEBChZCYXR0",
            "bGVSZXBvcnRQbGF5ZXJEYXRhEhQKDFNlbGZQbGF5ZXJJZBgBIAEoBRINCgVS",
            "b3VuZBgCIAEoBRINCgVGcmFtZRgDIAEoBRJDCg9QbGF5ZXJTdGF0ZURhdGEY",
            "BCADKAsyKi5HYW1lLlByb3RvYnVmLlR5cGVzLkJhdHRsZVJlcG9ydFN0YXRl",
            "RGF0YSpiChpGaWdodGluZ1Jvb21QbGF5ZXJQb3NpdGlvbhIPCgtOb19Qb3Np",
            "dGlvbhAAEgoKBlJlamVjdBABEg0KCVBsYXllcl8xUBACEg0KCVBsYXllcl8y",
            "UBADEgkKBUJlbmNoEAQqLQoURmlnaHRpbmdSb29tU3luY1R5cGUSDAoITG9j",
            "a3N0ZXAQABIHCgNPY2MQASp7ChtGaWdodGluZ1Jvb21CYXR0bGVHYW1lU3Rh",
            "dGUSDQoJVW5kZWZpbmVkEAASCwoHUHJlcGFyZRABEg4KCkdhbWVfU3RhcnQQ",
            "AhIQCgxCYXR0bGVfU3RhcnQQAxIPCgtSb3VuZF9TdGFydBAEEg0KCVJvdW5k",
            "X0VuZBAFKkwKDEJhdHRsZVJlc3VsdBIPCgtSZXN1bHRFcnJvchAAEgkKBVAx",
            "V2luEAESCQoFUDJXaW4QAhIICgREcmF3EAMSCwoHVGltZW91dBAEYgZwcm90",
            "bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Game.Protobuf.Types.FightingRoomPlayerPosition), typeof(global::Game.Protobuf.Types.FightingRoomSyncType), typeof(global::Game.Protobuf.Types.FightingRoomBattleGameState), typeof(global::Game.Protobuf.Types.BattleResult), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.CharacterData), global::Game.Protobuf.Types.CharacterData.Parser, new[]{ "CharacterId", "ShowName", "CharacterFlag" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.RegisterCharacterData), global::Game.Protobuf.Types.RegisterCharacterData.Parser, new[]{ "JoinRoomTicket", "CharacterData" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRoomCharacterStateData), global::Game.Protobuf.Types.FightingRoomCharacterStateData.Parser, new[]{ "CharacterId", "CharacterName", "RoleId", "WeaponId", "SubWeaponId", "HeadId", "ClothId", "BackpackId", "Hp", "Atk", "WalkSpeed", "SameMoveCorrection", "ComboCorrection", "AirComboCorrection", "IsReadyPlayAgain", "IsReady", "Position", "BattleGameState" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRoomBattleSetting), global::Game.Protobuf.Types.FightingRoomBattleSetting.Parser, new[]{ "Bo", "RoundTime", "InputDelayFrame", "InputFps", "RoundRandomSeeds" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRoomSetting), global::Game.Protobuf.Types.FightingRoomSetting.Parser, new[]{ "ErrorCode", "StageId", "SyncType", "CharactersInfo", "BattleSetting" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRoundData), global::Game.Protobuf.Types.FightingRoundData.Parser, new[]{ "WinnerCharacterId" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRoomStageStateData), global::Game.Protobuf.Types.FightingRoomStageStateData.Parser, new[]{ "IsPlanning", "StageId", "CurrentRound", "RoundData", "BattleSetting", "BattleGameState" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRoomPingData), global::Game.Protobuf.Types.FightingRoomPingData.Parser, new[]{ "Id", "PingTime", "PongTime" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.SignalCharacterSync), global::Game.Protobuf.Types.SignalCharacterSync.Parser, new[]{ "SyncFlag", "ShowName" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.CommandCharacterSync), global::Game.Protobuf.Types.CommandCharacterSync.Parser, new[]{ "IsYou", "CharacterData" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.RequestCreateFightingRoom), global::Game.Protobuf.Types.RequestCreateFightingRoom.Parser, new[]{ "RoomKey", "RoomSyncType", "BattleSetting" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.RequestJoinFightingRoom), global::Game.Protobuf.Types.RequestJoinFightingRoom.Parser, new[]{ "RoomId", "RoomKey" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.RequestLeaveFightingRoom), global::Game.Protobuf.Types.RequestLeaveFightingRoom.Parser, null, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.CommandFightingRoomSync), global::Game.Protobuf.Types.CommandFightingRoomSync.Parser, new[]{ "RoomId", "RoomMasterCharacterId", "RoomSyncType", "StageStateData", "CharacterStateDatas" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.CommandFightingRoomAction), global::Game.Protobuf.Types.CommandFightingRoomAction.Parser, new[]{ "ActionType", "CharacterId", "SyncIntId", "SyncPosition" }, null, new[]{ typeof(global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType) }, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.SignalFightingRoomAction), global::Game.Protobuf.Types.SignalFightingRoomAction.Parser, new[]{ "ActionType", "SyncIntId", "SyncPosition" }, null, new[]{ typeof(global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType) }, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRooomBattleKeyDownSync), global::Game.Protobuf.Types.FightingRooomBattleKeyDownSync.Parser, new[]{ "Round", "PlayerId", "FlagFromFrame", "KeyDownFlag", "LastReceiveAdvanceFrameX100" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.BattleActionDataLog), global::Game.Protobuf.Types.BattleActionDataLog.Parser, new[]{ "RoleId", "WeaponId", "SubWeaponId", "WeaponBrokenTimes", "SubWeaponBrokenTimes", "UsedThrowTimes", "UsedDeThrowTimes", "UsedReversalTimes", "CharacterId" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRoomBattleResult), global::Game.Protobuf.Types.FightingRoomBattleResult.Parser, new[]{ "ErrorCode", "TicketPasscode", "Result", "IsSuddenDeath", "BattleAction" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.ReportFrameData), global::Game.Protobuf.Types.ReportFrameData.Parser, new[]{ "Frame", "Key", "Location", "Rotation", "Hp", "State", "Move", "Animation", "KeyEventArray", "KeyPoolArray", "Event" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingPlayerReport), global::Game.Protobuf.Types.FightingPlayerReport.Parser, new[]{ "RoleId", "WeaponId", "SubWeaponId", "FrameAction" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.RoundPlayerReportData), global::Game.Protobuf.Types.RoundPlayerReportData.Parser, new[]{ "Round", "PlayerData" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.FightingRoomReport), global::Game.Protobuf.Types.FightingRoomReport.Parser, new[]{ "BattleReports" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.ProtoVector), global::Game.Protobuf.Types.ProtoVector.Parser, new[]{ "X", "Y", "Z" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.ProtoRotate), global::Game.Protobuf.Types.ProtoRotate.Parser, new[]{ "Pitch", "Yaw", "Roll" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.BattleReportStateData), global::Game.Protobuf.Types.BattleReportStateData.Parser, new[]{ "Vactor", "Rotate", "Hp", "FighterStateCtrlID", "KeyDownFlag", "KeyDownFlagLast", "AminID", "MoveID" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Game.Protobuf.Types.BattleReportPlayerData), global::Game.Protobuf.Types.BattleReportPlayerData.Parser, new[]{ "SelfPlayerId", "Round", "Frame", "PlayerStateData" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum FightingRoomPlayerPosition {
    /// <summary>
    /// 不在房內
    /// </summary>
    [pbr::OriginalName("No_Position")] NoPosition = 0,
    /// <summary>
    /// 無法進入
    /// </summary>
    [pbr::OriginalName("Reject")] Reject = 1,
    [pbr::OriginalName("Player_1P")] Player1P = 2,
    [pbr::OriginalName("Player_2P")] Player2P = 3,
    [pbr::OriginalName("Bench")] Bench = 4,
  }

  public enum FightingRoomSyncType {
    /// <summary>
    /// 鎖幀, 配合DelayFrame和Rollback使遊玩體感順暢
    /// </summary>
    [pbr::OriginalName("Lockstep")] Lockstep = 0,
    /// <summary>
    /// 樂觀並行控制(Optimistic Concurrency Control)
    /// </summary>
    [pbr::OriginalName("Occ")] Occ = 1,
  }

  /// <summary>
  /// RoomAction:BattleGameState SyncId
  /// </summary>
  public enum FightingRoomBattleGameState {
    [pbr::OriginalName("Undefined")] Undefined = 0,
    [pbr::OriginalName("Prepare")] Prepare = 1,
    [pbr::OriginalName("Game_Start")] GameStart = 2,
    [pbr::OriginalName("Battle_Start")] BattleStart = 3,
    [pbr::OriginalName("Round_Start")] RoundStart = 4,
    [pbr::OriginalName("Round_End")] RoundEnd = 5,
  }

  public enum BattleResult {
    [pbr::OriginalName("ResultError")] ResultError = 0,
    [pbr::OriginalName("P1Win")] P1Win = 1,
    [pbr::OriginalName("P2Win")] P2Win = 2,
    [pbr::OriginalName("Draw")] Draw = 3,
    [pbr::OriginalName("Timeout")] Timeout = 4,
  }

  #endregion

  #region Messages
  public sealed partial class CharacterData : pb::IMessage<CharacterData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CharacterData> _parser = new pb::MessageParser<CharacterData>(() => new CharacterData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CharacterData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CharacterData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CharacterData(CharacterData other) : this() {
      characterId_ = other.characterId_;
      showName_ = other.showName_;
      characterFlag_ = other.characterFlag_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    CharacterData pb::IDeepCloneable<CharacterData>.Clone() {
      return new CharacterData(this);
    }

    /// <summary>Field number for the "Character_Id" field.</summary>
    public const int CharacterIdFieldNumber = 1;
    private ulong characterId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ulong CharacterId {
      get { return characterId_; }
      set {
        characterId_ = value;
      }
    }

    /// <summary>Field number for the "Show_Name" field.</summary>
    public const int ShowNameFieldNumber = 2;
    private string showName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string ShowName {
      get { return showName_; }
      set {
        showName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Character_Flag" field.</summary>
    public const int CharacterFlagFieldNumber = 3;
    private uint characterFlag_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint CharacterFlag {
      get { return characterFlag_; }
      set {
        characterFlag_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<CharacterData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<CharacterData>.Equals(CharacterData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (CharacterId != other.CharacterId) return false;
      if (ShowName != other.ShowName) return false;
      if (CharacterFlag != other.CharacterFlag) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (CharacterId != 0UL) hash ^= CharacterId.GetHashCode();
      if (ShowName.Length != 0) hash ^= ShowName.GetHashCode();
      if (CharacterFlag != 0) hash ^= CharacterFlag.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (CharacterId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(CharacterId);
      }
      if (ShowName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(ShowName);
      }
      if (CharacterFlag != 0) {
        output.WriteRawTag(24);
        output.WriteUInt32(CharacterFlag);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (CharacterId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(CharacterId);
      }
      if (ShowName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(ShowName);
      }
      if (CharacterFlag != 0) {
        output.WriteRawTag(24);
        output.WriteUInt32(CharacterFlag);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (CharacterId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(CharacterId);
      }
      if (ShowName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ShowName);
      }
      if (CharacterFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(CharacterFlag);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<CharacterData>.MergeFrom(CharacterData other) {
      if (other == null) {
        return;
      }
      if (other.CharacterId != 0UL) {
        CharacterId = other.CharacterId;
      }
      if (other.ShowName.Length != 0) {
        ShowName = other.ShowName;
      }
      if (other.CharacterFlag != 0) {
        CharacterFlag = other.CharacterFlag;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            CharacterId = input.ReadUInt64();
            break;
          }
          case 18: {
            ShowName = input.ReadString();
            break;
          }
          case 24: {
            CharacterFlag = input.ReadUInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            CharacterId = input.ReadUInt64();
            break;
          }
          case 18: {
            ShowName = input.ReadString();
            break;
          }
          case 24: {
            CharacterFlag = input.ReadUInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class RegisterCharacterData : pb::IMessage<RegisterCharacterData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RegisterCharacterData> _parser = new pb::MessageParser<RegisterCharacterData>(() => new RegisterCharacterData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<RegisterCharacterData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RegisterCharacterData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RegisterCharacterData(RegisterCharacterData other) : this() {
      joinRoomTicket_ = other.joinRoomTicket_ != null ? other.joinRoomTicket_.Clone() : null;
      characterData_ = other.characterData_ != null ? other.characterData_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    RegisterCharacterData pb::IDeepCloneable<RegisterCharacterData>.Clone() {
      return new RegisterCharacterData(this);
    }

    /// <summary>Field number for the "Join_Room_Ticket" field.</summary>
    public const int JoinRoomTicketFieldNumber = 1;
    private global::Game.Protobuf.Types.RequestJoinFightingRoom joinRoomTicket_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.RequestJoinFightingRoom JoinRoomTicket {
      get { return joinRoomTicket_; }
      set {
        joinRoomTicket_ = value;
      }
    }

    /// <summary>Field number for the "Character_Data" field.</summary>
    public const int CharacterDataFieldNumber = 2;
    private global::Game.Protobuf.Types.CharacterData characterData_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.CharacterData CharacterData {
      get { return characterData_; }
      set {
        characterData_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<RegisterCharacterData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<RegisterCharacterData>.Equals(RegisterCharacterData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(JoinRoomTicket, other.JoinRoomTicket)) return false;
      if (!object.Equals(CharacterData, other.CharacterData)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (joinRoomTicket_ != null) hash ^= JoinRoomTicket.GetHashCode();
      if (characterData_ != null) hash ^= CharacterData.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (joinRoomTicket_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(JoinRoomTicket);
      }
      if (characterData_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(CharacterData);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (joinRoomTicket_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(JoinRoomTicket);
      }
      if (characterData_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(CharacterData);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (joinRoomTicket_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(JoinRoomTicket);
      }
      if (characterData_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(CharacterData);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<RegisterCharacterData>.MergeFrom(RegisterCharacterData other) {
      if (other == null) {
        return;
      }
      if (other.joinRoomTicket_ != null) {
        if (joinRoomTicket_ == null) {
          JoinRoomTicket = new global::Game.Protobuf.Types.RequestJoinFightingRoom();
        }
        JoinRoomTicket.MergeFrom(other.JoinRoomTicket);
      }
      if (other.characterData_ != null) {
        if (characterData_ == null) {
          CharacterData = new global::Game.Protobuf.Types.CharacterData();
        }
        CharacterData.MergeFrom(other.CharacterData);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (joinRoomTicket_ == null) {
              JoinRoomTicket = new global::Game.Protobuf.Types.RequestJoinFightingRoom();
            }
            input.ReadMessage(JoinRoomTicket);
            break;
          }
          case 18: {
            if (characterData_ == null) {
              CharacterData = new global::Game.Protobuf.Types.CharacterData();
            }
            input.ReadMessage(CharacterData);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (joinRoomTicket_ == null) {
              JoinRoomTicket = new global::Game.Protobuf.Types.RequestJoinFightingRoom();
            }
            input.ReadMessage(JoinRoomTicket);
            break;
          }
          case 18: {
            if (characterData_ == null) {
              CharacterData = new global::Game.Protobuf.Types.CharacterData();
            }
            input.ReadMessage(CharacterData);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FightingRoomCharacterStateData : pb::IMessage<FightingRoomCharacterStateData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRoomCharacterStateData> _parser = new pb::MessageParser<FightingRoomCharacterStateData>(() => new FightingRoomCharacterStateData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRoomCharacterStateData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomCharacterStateData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomCharacterStateData(FightingRoomCharacterStateData other) : this() {
      characterId_ = other.characterId_;
      characterName_ = other.characterName_;
      roleId_ = other.roleId_;
      weaponId_ = other.weaponId_;
      subWeaponId_ = other.subWeaponId_;
      headId_ = other.headId_;
      clothId_ = other.clothId_;
      backpackId_ = other.backpackId_;
      hp_ = other.hp_;
      atk_ = other.atk_;
      walkSpeed_ = other.walkSpeed_;
      sameMoveCorrection_ = other.sameMoveCorrection_;
      comboCorrection_ = other.comboCorrection_.Clone();
      airComboCorrection_ = other.airComboCorrection_.Clone();
      isReadyPlayAgain_ = other.isReadyPlayAgain_;
      isReady_ = other.isReady_;
      position_ = other.position_;
      battleGameState_ = other.battleGameState_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRoomCharacterStateData pb::IDeepCloneable<FightingRoomCharacterStateData>.Clone() {
      return new FightingRoomCharacterStateData(this);
    }

    /// <summary>Field number for the "Character_Id" field.</summary>
    public const int CharacterIdFieldNumber = 1;
    private ulong characterId_;
    /// <summary>
    /// 角色 id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ulong CharacterId {
      get { return characterId_; }
      set {
        characterId_ = value;
      }
    }

    /// <summary>Field number for the "Character_Name" field.</summary>
    public const int CharacterNameFieldNumber = 2;
    private string characterName_ = "";
    /// <summary>
    /// 角色名稱
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string CharacterName {
      get { return characterName_; }
      set {
        characterName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Role_Id" field.</summary>
    public const int RoleIdFieldNumber = 3;
    private int roleId_;
    /// <summary>
    /// 角色 id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int RoleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    /// <summary>Field number for the "Weapon_Id" field.</summary>
    public const int WeaponIdFieldNumber = 4;
    private int weaponId_;
    /// <summary>
    /// 武器 id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int WeaponId {
      get { return weaponId_; }
      set {
        weaponId_ = value;
      }
    }

    /// <summary>Field number for the "Sub_Weapon_Id" field.</summary>
    public const int SubWeaponIdFieldNumber = 5;
    private int subWeaponId_;
    /// <summary>
    /// 副武器 id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SubWeaponId {
      get { return subWeaponId_; }
      set {
        subWeaponId_ = value;
      }
    }

    /// <summary>Field number for the "Head_Id" field.</summary>
    public const int HeadIdFieldNumber = 6;
    private int headId_;
    /// <summary>
    /// 頭飾紙娃娃 id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int HeadId {
      get { return headId_; }
      set {
        headId_ = value;
      }
    }

    /// <summary>Field number for the "Cloth_Id" field.</summary>
    public const int ClothIdFieldNumber = 7;
    private int clothId_;
    /// <summary>
    /// 衣服紙娃娃 id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ClothId {
      get { return clothId_; }
      set {
        clothId_ = value;
      }
    }

    /// <summary>Field number for the "Backpack_Id" field.</summary>
    public const int BackpackIdFieldNumber = 8;
    private int backpackId_;
    /// <summary>
    /// 背包紙娃娃 id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int BackpackId {
      get { return backpackId_; }
      set {
        backpackId_ = value;
      }
    }

    /// <summary>Field number for the "Hp" field.</summary>
    public const int HpFieldNumber = 9;
    private int hp_;
    /// <summary>
    /// 血量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Hp {
      get { return hp_; }
      set {
        hp_ = value;
      }
    }

    /// <summary>Field number for the "Atk" field.</summary>
    public const int AtkFieldNumber = 10;
    private int atk_;
    /// <summary>
    /// 攻擊力
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Atk {
      get { return atk_; }
      set {
        atk_ = value;
      }
    }

    /// <summary>Field number for the "Walk_Speed" field.</summary>
    public const int WalkSpeedFieldNumber = 11;
    private int walkSpeed_;
    /// <summary>
    /// 移動速度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int WalkSpeed {
      get { return walkSpeed_; }
      set {
        walkSpeed_ = value;
      }
    }

    /// <summary>Field number for the "Same_Move_Correction" field.</summary>
    public const int SameMoveCorrectionFieldNumber = 12;
    private int sameMoveCorrection_;
    /// <summary>
    /// 同招式補正
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SameMoveCorrection {
      get { return sameMoveCorrection_; }
      set {
        sameMoveCorrection_ = value;
      }
    }

    /// <summary>Field number for the "Combo_Correction" field.</summary>
    public const int ComboCorrectionFieldNumber = 13;
    private static readonly pb::FieldCodec<int> _repeated_comboCorrection_codec
        = pb::FieldCodec.ForInt32(106);
    private readonly pbc::RepeatedField<int> comboCorrection_ = new pbc::RepeatedField<int>();
    /// <summary>
    /// combo傷害補正
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<int> ComboCorrection {
      get { return comboCorrection_; }
    }

    /// <summary>Field number for the "Air_Combo_Correction" field.</summary>
    public const int AirComboCorrectionFieldNumber = 14;
    private static readonly pb::FieldCodec<int> _repeated_airComboCorrection_codec
        = pb::FieldCodec.ForInt32(114);
    private readonly pbc::RepeatedField<int> airComboCorrection_ = new pbc::RepeatedField<int>();
    /// <summary>
    /// 空中combo傷害補正
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<int> AirComboCorrection {
      get { return airComboCorrection_; }
    }

    /// <summary>Field number for the "Is_Ready_Play_Again" field.</summary>
    public const int IsReadyPlayAgainFieldNumber = 15;
    private int isReadyPlayAgain_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int IsReadyPlayAgain {
      get { return isReadyPlayAgain_; }
      set {
        isReadyPlayAgain_ = value;
      }
    }

    /// <summary>Field number for the "Is_Ready" field.</summary>
    public const int IsReadyFieldNumber = 16;
    private int isReady_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int IsReady {
      get { return isReady_; }
      set {
        isReady_ = value;
      }
    }

    /// <summary>Field number for the "Position" field.</summary>
    public const int PositionFieldNumber = 17;
    private global::Game.Protobuf.Types.FightingRoomPlayerPosition position_ = global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomPlayerPosition Position {
      get { return position_; }
      set {
        position_ = value;
      }
    }

    /// <summary>Field number for the "Battle_Game_State" field.</summary>
    public const int BattleGameStateFieldNumber = 18;
    private global::Game.Protobuf.Types.FightingRoomBattleGameState battleGameState_ = global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomBattleGameState BattleGameState {
      get { return battleGameState_; }
      set {
        battleGameState_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRoomCharacterStateData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRoomCharacterStateData>.Equals(FightingRoomCharacterStateData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (CharacterId != other.CharacterId) return false;
      if (CharacterName != other.CharacterName) return false;
      if (RoleId != other.RoleId) return false;
      if (WeaponId != other.WeaponId) return false;
      if (SubWeaponId != other.SubWeaponId) return false;
      if (HeadId != other.HeadId) return false;
      if (ClothId != other.ClothId) return false;
      if (BackpackId != other.BackpackId) return false;
      if (Hp != other.Hp) return false;
      if (Atk != other.Atk) return false;
      if (WalkSpeed != other.WalkSpeed) return false;
      if (SameMoveCorrection != other.SameMoveCorrection) return false;
      if(!comboCorrection_.Equals(other.comboCorrection_)) return false;
      if(!airComboCorrection_.Equals(other.airComboCorrection_)) return false;
      if (IsReadyPlayAgain != other.IsReadyPlayAgain) return false;
      if (IsReady != other.IsReady) return false;
      if (Position != other.Position) return false;
      if (BattleGameState != other.BattleGameState) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (CharacterId != 0UL) hash ^= CharacterId.GetHashCode();
      if (CharacterName.Length != 0) hash ^= CharacterName.GetHashCode();
      if (RoleId != 0) hash ^= RoleId.GetHashCode();
      if (WeaponId != 0) hash ^= WeaponId.GetHashCode();
      if (SubWeaponId != 0) hash ^= SubWeaponId.GetHashCode();
      if (HeadId != 0) hash ^= HeadId.GetHashCode();
      if (ClothId != 0) hash ^= ClothId.GetHashCode();
      if (BackpackId != 0) hash ^= BackpackId.GetHashCode();
      if (Hp != 0) hash ^= Hp.GetHashCode();
      if (Atk != 0) hash ^= Atk.GetHashCode();
      if (WalkSpeed != 0) hash ^= WalkSpeed.GetHashCode();
      if (SameMoveCorrection != 0) hash ^= SameMoveCorrection.GetHashCode();
      hash ^= comboCorrection_.GetHashCode();
      hash ^= airComboCorrection_.GetHashCode();
      if (IsReadyPlayAgain != 0) hash ^= IsReadyPlayAgain.GetHashCode();
      if (IsReady != 0) hash ^= IsReady.GetHashCode();
      if (Position != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) hash ^= Position.GetHashCode();
      if (BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) hash ^= BattleGameState.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (CharacterId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(CharacterId);
      }
      if (CharacterName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(CharacterName);
      }
      if (RoleId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(RoleId);
      }
      if (WeaponId != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(WeaponId);
      }
      if (SubWeaponId != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(SubWeaponId);
      }
      if (HeadId != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(HeadId);
      }
      if (ClothId != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(ClothId);
      }
      if (BackpackId != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(BackpackId);
      }
      if (Hp != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(Hp);
      }
      if (Atk != 0) {
        output.WriteRawTag(80);
        output.WriteInt32(Atk);
      }
      if (WalkSpeed != 0) {
        output.WriteRawTag(88);
        output.WriteInt32(WalkSpeed);
      }
      if (SameMoveCorrection != 0) {
        output.WriteRawTag(96);
        output.WriteInt32(SameMoveCorrection);
      }
      comboCorrection_.WriteTo(output, _repeated_comboCorrection_codec);
      airComboCorrection_.WriteTo(output, _repeated_airComboCorrection_codec);
      if (IsReadyPlayAgain != 0) {
        output.WriteRawTag(120);
        output.WriteInt32(IsReadyPlayAgain);
      }
      if (IsReady != 0) {
        output.WriteRawTag(128, 1);
        output.WriteInt32(IsReady);
      }
      if (Position != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        output.WriteRawTag(136, 1);
        output.WriteEnum((int) Position);
      }
      if (BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) {
        output.WriteRawTag(144, 1);
        output.WriteEnum((int) BattleGameState);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (CharacterId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(CharacterId);
      }
      if (CharacterName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(CharacterName);
      }
      if (RoleId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(RoleId);
      }
      if (WeaponId != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(WeaponId);
      }
      if (SubWeaponId != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(SubWeaponId);
      }
      if (HeadId != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(HeadId);
      }
      if (ClothId != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(ClothId);
      }
      if (BackpackId != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(BackpackId);
      }
      if (Hp != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(Hp);
      }
      if (Atk != 0) {
        output.WriteRawTag(80);
        output.WriteInt32(Atk);
      }
      if (WalkSpeed != 0) {
        output.WriteRawTag(88);
        output.WriteInt32(WalkSpeed);
      }
      if (SameMoveCorrection != 0) {
        output.WriteRawTag(96);
        output.WriteInt32(SameMoveCorrection);
      }
      comboCorrection_.WriteTo(ref output, _repeated_comboCorrection_codec);
      airComboCorrection_.WriteTo(ref output, _repeated_airComboCorrection_codec);
      if (IsReadyPlayAgain != 0) {
        output.WriteRawTag(120);
        output.WriteInt32(IsReadyPlayAgain);
      }
      if (IsReady != 0) {
        output.WriteRawTag(128, 1);
        output.WriteInt32(IsReady);
      }
      if (Position != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        output.WriteRawTag(136, 1);
        output.WriteEnum((int) Position);
      }
      if (BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) {
        output.WriteRawTag(144, 1);
        output.WriteEnum((int) BattleGameState);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (CharacterId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(CharacterId);
      }
      if (CharacterName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CharacterName);
      }
      if (RoleId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoleId);
      }
      if (WeaponId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(WeaponId);
      }
      if (SubWeaponId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SubWeaponId);
      }
      if (HeadId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(HeadId);
      }
      if (ClothId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ClothId);
      }
      if (BackpackId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(BackpackId);
      }
      if (Hp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Hp);
      }
      if (Atk != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Atk);
      }
      if (WalkSpeed != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(WalkSpeed);
      }
      if (SameMoveCorrection != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SameMoveCorrection);
      }
      size += comboCorrection_.CalculateSize(_repeated_comboCorrection_codec);
      size += airComboCorrection_.CalculateSize(_repeated_airComboCorrection_codec);
      if (IsReadyPlayAgain != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(IsReadyPlayAgain);
      }
      if (IsReady != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(IsReady);
      }
      if (Position != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        size += 2 + pb::CodedOutputStream.ComputeEnumSize((int) Position);
      }
      if (BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) {
        size += 2 + pb::CodedOutputStream.ComputeEnumSize((int) BattleGameState);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRoomCharacterStateData>.MergeFrom(FightingRoomCharacterStateData other) {
      if (other == null) {
        return;
      }
      if (other.CharacterId != 0UL) {
        CharacterId = other.CharacterId;
      }
      if (other.CharacterName.Length != 0) {
        CharacterName = other.CharacterName;
      }
      if (other.RoleId != 0) {
        RoleId = other.RoleId;
      }
      if (other.WeaponId != 0) {
        WeaponId = other.WeaponId;
      }
      if (other.SubWeaponId != 0) {
        SubWeaponId = other.SubWeaponId;
      }
      if (other.HeadId != 0) {
        HeadId = other.HeadId;
      }
      if (other.ClothId != 0) {
        ClothId = other.ClothId;
      }
      if (other.BackpackId != 0) {
        BackpackId = other.BackpackId;
      }
      if (other.Hp != 0) {
        Hp = other.Hp;
      }
      if (other.Atk != 0) {
        Atk = other.Atk;
      }
      if (other.WalkSpeed != 0) {
        WalkSpeed = other.WalkSpeed;
      }
      if (other.SameMoveCorrection != 0) {
        SameMoveCorrection = other.SameMoveCorrection;
      }
      comboCorrection_.Add(other.comboCorrection_);
      airComboCorrection_.Add(other.airComboCorrection_);
      if (other.IsReadyPlayAgain != 0) {
        IsReadyPlayAgain = other.IsReadyPlayAgain;
      }
      if (other.IsReady != 0) {
        IsReady = other.IsReady;
      }
      if (other.Position != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        Position = other.Position;
      }
      if (other.BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) {
        BattleGameState = other.BattleGameState;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            CharacterId = input.ReadUInt64();
            break;
          }
          case 18: {
            CharacterName = input.ReadString();
            break;
          }
          case 24: {
            RoleId = input.ReadInt32();
            break;
          }
          case 32: {
            WeaponId = input.ReadInt32();
            break;
          }
          case 40: {
            SubWeaponId = input.ReadInt32();
            break;
          }
          case 48: {
            HeadId = input.ReadInt32();
            break;
          }
          case 56: {
            ClothId = input.ReadInt32();
            break;
          }
          case 64: {
            BackpackId = input.ReadInt32();
            break;
          }
          case 72: {
            Hp = input.ReadInt32();
            break;
          }
          case 80: {
            Atk = input.ReadInt32();
            break;
          }
          case 88: {
            WalkSpeed = input.ReadInt32();
            break;
          }
          case 96: {
            SameMoveCorrection = input.ReadInt32();
            break;
          }
          case 106:
          case 104: {
            comboCorrection_.AddEntriesFrom(input, _repeated_comboCorrection_codec);
            break;
          }
          case 114:
          case 112: {
            airComboCorrection_.AddEntriesFrom(input, _repeated_airComboCorrection_codec);
            break;
          }
          case 120: {
            IsReadyPlayAgain = input.ReadInt32();
            break;
          }
          case 128: {
            IsReady = input.ReadInt32();
            break;
          }
          case 136: {
            Position = (global::Game.Protobuf.Types.FightingRoomPlayerPosition) input.ReadEnum();
            break;
          }
          case 144: {
            BattleGameState = (global::Game.Protobuf.Types.FightingRoomBattleGameState) input.ReadEnum();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            CharacterId = input.ReadUInt64();
            break;
          }
          case 18: {
            CharacterName = input.ReadString();
            break;
          }
          case 24: {
            RoleId = input.ReadInt32();
            break;
          }
          case 32: {
            WeaponId = input.ReadInt32();
            break;
          }
          case 40: {
            SubWeaponId = input.ReadInt32();
            break;
          }
          case 48: {
            HeadId = input.ReadInt32();
            break;
          }
          case 56: {
            ClothId = input.ReadInt32();
            break;
          }
          case 64: {
            BackpackId = input.ReadInt32();
            break;
          }
          case 72: {
            Hp = input.ReadInt32();
            break;
          }
          case 80: {
            Atk = input.ReadInt32();
            break;
          }
          case 88: {
            WalkSpeed = input.ReadInt32();
            break;
          }
          case 96: {
            SameMoveCorrection = input.ReadInt32();
            break;
          }
          case 106:
          case 104: {
            comboCorrection_.AddEntriesFrom(ref input, _repeated_comboCorrection_codec);
            break;
          }
          case 114:
          case 112: {
            airComboCorrection_.AddEntriesFrom(ref input, _repeated_airComboCorrection_codec);
            break;
          }
          case 120: {
            IsReadyPlayAgain = input.ReadInt32();
            break;
          }
          case 128: {
            IsReady = input.ReadInt32();
            break;
          }
          case 136: {
            Position = (global::Game.Protobuf.Types.FightingRoomPlayerPosition) input.ReadEnum();
            break;
          }
          case 144: {
            BattleGameState = (global::Game.Protobuf.Types.FightingRoomBattleGameState) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FightingRoomBattleSetting : pb::IMessage<FightingRoomBattleSetting>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRoomBattleSetting> _parser = new pb::MessageParser<FightingRoomBattleSetting>(() => new FightingRoomBattleSetting());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRoomBattleSetting> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomBattleSetting() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomBattleSetting(FightingRoomBattleSetting other) : this() {
      bo_ = other.bo_;
      roundTime_ = other.roundTime_;
      inputDelayFrame_ = other.inputDelayFrame_;
      inputFps_ = other.inputFps_;
      roundRandomSeeds_ = other.roundRandomSeeds_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRoomBattleSetting pb::IDeepCloneable<FightingRoomBattleSetting>.Clone() {
      return new FightingRoomBattleSetting(this);
    }

    /// <summary>Field number for the "Bo" field.</summary>
    public const int BoFieldNumber = 1;
    private int bo_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Bo {
      get { return bo_; }
      set {
        bo_ = value;
      }
    }

    /// <summary>Field number for the "Round_Time" field.</summary>
    public const int RoundTimeFieldNumber = 2;
    private int roundTime_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int RoundTime {
      get { return roundTime_; }
      set {
        roundTime_ = value;
      }
    }

    /// <summary>Field number for the "Input_Delay_Frame" field.</summary>
    public const int InputDelayFrameFieldNumber = 3;
    private int inputDelayFrame_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int InputDelayFrame {
      get { return inputDelayFrame_; }
      set {
        inputDelayFrame_ = value;
      }
    }

    /// <summary>Field number for the "Input_Fps" field.</summary>
    public const int InputFpsFieldNumber = 4;
    private int inputFps_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int InputFps {
      get { return inputFps_; }
      set {
        inputFps_ = value;
      }
    }

    /// <summary>Field number for the "Round_Random_Seeds" field.</summary>
    public const int RoundRandomSeedsFieldNumber = 5;
    private static readonly pb::FieldCodec<long> _repeated_roundRandomSeeds_codec
        = pb::FieldCodec.ForInt64(42);
    private readonly pbc::RepeatedField<long> roundRandomSeeds_ = new pbc::RepeatedField<long>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<long> RoundRandomSeeds {
      get { return roundRandomSeeds_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRoomBattleSetting> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRoomBattleSetting>.Equals(FightingRoomBattleSetting other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Bo != other.Bo) return false;
      if (RoundTime != other.RoundTime) return false;
      if (InputDelayFrame != other.InputDelayFrame) return false;
      if (InputFps != other.InputFps) return false;
      if(!roundRandomSeeds_.Equals(other.roundRandomSeeds_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Bo != 0) hash ^= Bo.GetHashCode();
      if (RoundTime != 0) hash ^= RoundTime.GetHashCode();
      if (InputDelayFrame != 0) hash ^= InputDelayFrame.GetHashCode();
      if (InputFps != 0) hash ^= InputFps.GetHashCode();
      hash ^= roundRandomSeeds_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Bo != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Bo);
      }
      if (RoundTime != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(RoundTime);
      }
      if (InputDelayFrame != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(InputDelayFrame);
      }
      if (InputFps != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(InputFps);
      }
      roundRandomSeeds_.WriteTo(output, _repeated_roundRandomSeeds_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Bo != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Bo);
      }
      if (RoundTime != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(RoundTime);
      }
      if (InputDelayFrame != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(InputDelayFrame);
      }
      if (InputFps != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(InputFps);
      }
      roundRandomSeeds_.WriteTo(ref output, _repeated_roundRandomSeeds_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (Bo != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Bo);
      }
      if (RoundTime != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoundTime);
      }
      if (InputDelayFrame != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(InputDelayFrame);
      }
      if (InputFps != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(InputFps);
      }
      size += roundRandomSeeds_.CalculateSize(_repeated_roundRandomSeeds_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRoomBattleSetting>.MergeFrom(FightingRoomBattleSetting other) {
      if (other == null) {
        return;
      }
      if (other.Bo != 0) {
        Bo = other.Bo;
      }
      if (other.RoundTime != 0) {
        RoundTime = other.RoundTime;
      }
      if (other.InputDelayFrame != 0) {
        InputDelayFrame = other.InputDelayFrame;
      }
      if (other.InputFps != 0) {
        InputFps = other.InputFps;
      }
      roundRandomSeeds_.Add(other.roundRandomSeeds_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Bo = input.ReadInt32();
            break;
          }
          case 16: {
            RoundTime = input.ReadInt32();
            break;
          }
          case 24: {
            InputDelayFrame = input.ReadInt32();
            break;
          }
          case 32: {
            InputFps = input.ReadInt32();
            break;
          }
          case 42:
          case 40: {
            roundRandomSeeds_.AddEntriesFrom(input, _repeated_roundRandomSeeds_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Bo = input.ReadInt32();
            break;
          }
          case 16: {
            RoundTime = input.ReadInt32();
            break;
          }
          case 24: {
            InputDelayFrame = input.ReadInt32();
            break;
          }
          case 32: {
            InputFps = input.ReadInt32();
            break;
          }
          case 42:
          case 40: {
            roundRandomSeeds_.AddEntriesFrom(ref input, _repeated_roundRandomSeeds_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FightingRoomSetting : pb::IMessage<FightingRoomSetting>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRoomSetting> _parser = new pb::MessageParser<FightingRoomSetting>(() => new FightingRoomSetting());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRoomSetting> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomSetting() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomSetting(FightingRoomSetting other) : this() {
      errorCode_ = other.errorCode_;
      stageId_ = other.stageId_;
      syncType_ = other.syncType_;
      charactersInfo_ = other.charactersInfo_.Clone();
      battleSetting_ = other.battleSetting_ != null ? other.battleSetting_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRoomSetting pb::IDeepCloneable<FightingRoomSetting>.Clone() {
      return new FightingRoomSetting(this);
    }

    /// <summary>Field number for the "Error_Code" field.</summary>
    public const int ErrorCodeFieldNumber = 1;
    private int errorCode_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ErrorCode {
      get { return errorCode_; }
      set {
        errorCode_ = value;
      }
    }

    /// <summary>Field number for the "Stage_Id" field.</summary>
    public const int StageIdFieldNumber = 2;
    private int stageId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int StageId {
      get { return stageId_; }
      set {
        stageId_ = value;
      }
    }

    /// <summary>Field number for the "Sync_Type" field.</summary>
    public const int SyncTypeFieldNumber = 3;
    private global::Game.Protobuf.Types.FightingRoomSyncType syncType_ = global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomSyncType SyncType {
      get { return syncType_; }
      set {
        syncType_ = value;
      }
    }

    /// <summary>Field number for the "Characters_Info" field.</summary>
    public const int CharactersInfoFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Game.Protobuf.Types.FightingRoomCharacterStateData> _repeated_charactersInfo_codec
        = pb::FieldCodec.ForMessage(34, global::Game.Protobuf.Types.FightingRoomCharacterStateData.Parser);
    private readonly pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoomCharacterStateData> charactersInfo_ = new pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoomCharacterStateData>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoomCharacterStateData> CharactersInfo {
      get { return charactersInfo_; }
    }

    /// <summary>Field number for the "Battle_Setting" field.</summary>
    public const int BattleSettingFieldNumber = 5;
    private global::Game.Protobuf.Types.FightingRoomBattleSetting battleSetting_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomBattleSetting BattleSetting {
      get { return battleSetting_; }
      set {
        battleSetting_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRoomSetting> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRoomSetting>.Equals(FightingRoomSetting other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ErrorCode != other.ErrorCode) return false;
      if (StageId != other.StageId) return false;
      if (SyncType != other.SyncType) return false;
      if(!charactersInfo_.Equals(other.charactersInfo_)) return false;
      if (!object.Equals(BattleSetting, other.BattleSetting)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ErrorCode != 0) hash ^= ErrorCode.GetHashCode();
      if (StageId != 0) hash ^= StageId.GetHashCode();
      if (SyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) hash ^= SyncType.GetHashCode();
      hash ^= charactersInfo_.GetHashCode();
      if (battleSetting_ != null) hash ^= BattleSetting.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (ErrorCode != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ErrorCode);
      }
      if (StageId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(StageId);
      }
      if (SyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        output.WriteRawTag(24);
        output.WriteEnum((int) SyncType);
      }
      charactersInfo_.WriteTo(output, _repeated_charactersInfo_codec);
      if (battleSetting_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(BattleSetting);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ErrorCode != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ErrorCode);
      }
      if (StageId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(StageId);
      }
      if (SyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        output.WriteRawTag(24);
        output.WriteEnum((int) SyncType);
      }
      charactersInfo_.WriteTo(ref output, _repeated_charactersInfo_codec);
      if (battleSetting_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(BattleSetting);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (ErrorCode != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ErrorCode);
      }
      if (StageId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(StageId);
      }
      if (SyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) SyncType);
      }
      size += charactersInfo_.CalculateSize(_repeated_charactersInfo_codec);
      if (battleSetting_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(BattleSetting);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRoomSetting>.MergeFrom(FightingRoomSetting other) {
      if (other == null) {
        return;
      }
      if (other.ErrorCode != 0) {
        ErrorCode = other.ErrorCode;
      }
      if (other.StageId != 0) {
        StageId = other.StageId;
      }
      if (other.SyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        SyncType = other.SyncType;
      }
      charactersInfo_.Add(other.charactersInfo_);
      if (other.battleSetting_ != null) {
        if (battleSetting_ == null) {
          BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
        }
        BattleSetting.MergeFrom(other.BattleSetting);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            ErrorCode = input.ReadInt32();
            break;
          }
          case 16: {
            StageId = input.ReadInt32();
            break;
          }
          case 24: {
            SyncType = (global::Game.Protobuf.Types.FightingRoomSyncType) input.ReadEnum();
            break;
          }
          case 34: {
            charactersInfo_.AddEntriesFrom(input, _repeated_charactersInfo_codec);
            break;
          }
          case 42: {
            if (battleSetting_ == null) {
              BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
            }
            input.ReadMessage(BattleSetting);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            ErrorCode = input.ReadInt32();
            break;
          }
          case 16: {
            StageId = input.ReadInt32();
            break;
          }
          case 24: {
            SyncType = (global::Game.Protobuf.Types.FightingRoomSyncType) input.ReadEnum();
            break;
          }
          case 34: {
            charactersInfo_.AddEntriesFrom(ref input, _repeated_charactersInfo_codec);
            break;
          }
          case 42: {
            if (battleSetting_ == null) {
              BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
            }
            input.ReadMessage(BattleSetting);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FightingRoundData : pb::IMessage<FightingRoundData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRoundData> _parser = new pb::MessageParser<FightingRoundData>(() => new FightingRoundData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRoundData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[5]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoundData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoundData(FightingRoundData other) : this() {
      winnerCharacterId_ = other.winnerCharacterId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRoundData pb::IDeepCloneable<FightingRoundData>.Clone() {
      return new FightingRoundData(this);
    }

    /// <summary>Field number for the "Winner_Character_Id" field.</summary>
    public const int WinnerCharacterIdFieldNumber = 1;
    private int winnerCharacterId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int WinnerCharacterId {
      get { return winnerCharacterId_; }
      set {
        winnerCharacterId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRoundData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRoundData>.Equals(FightingRoundData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (WinnerCharacterId != other.WinnerCharacterId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (WinnerCharacterId != 0) hash ^= WinnerCharacterId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (WinnerCharacterId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(WinnerCharacterId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (WinnerCharacterId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(WinnerCharacterId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (WinnerCharacterId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(WinnerCharacterId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRoundData>.MergeFrom(FightingRoundData other) {
      if (other == null) {
        return;
      }
      if (other.WinnerCharacterId != 0) {
        WinnerCharacterId = other.WinnerCharacterId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            WinnerCharacterId = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            WinnerCharacterId = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FightingRoomStageStateData : pb::IMessage<FightingRoomStageStateData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRoomStageStateData> _parser = new pb::MessageParser<FightingRoomStageStateData>(() => new FightingRoomStageStateData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRoomStageStateData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[6]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomStageStateData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomStageStateData(FightingRoomStageStateData other) : this() {
      isPlanning_ = other.isPlanning_;
      stageId_ = other.stageId_;
      currentRound_ = other.currentRound_;
      roundData_ = other.roundData_.Clone();
      battleSetting_ = other.battleSetting_ != null ? other.battleSetting_.Clone() : null;
      battleGameState_ = other.battleGameState_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRoomStageStateData pb::IDeepCloneable<FightingRoomStageStateData>.Clone() {
      return new FightingRoomStageStateData(this);
    }

    /// <summary>Field number for the "Is_Planning" field.</summary>
    public const int IsPlanningFieldNumber = 1;
    private int isPlanning_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int IsPlanning {
      get { return isPlanning_; }
      set {
        isPlanning_ = value;
      }
    }

    /// <summary>Field number for the "Stage_Id" field.</summary>
    public const int StageIdFieldNumber = 2;
    private int stageId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int StageId {
      get { return stageId_; }
      set {
        stageId_ = value;
      }
    }

    /// <summary>Field number for the "Current_Round" field.</summary>
    public const int CurrentRoundFieldNumber = 3;
    private int currentRound_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CurrentRound {
      get { return currentRound_; }
      set {
        currentRound_ = value;
      }
    }

    /// <summary>Field number for the "Round_Data" field.</summary>
    public const int RoundDataFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Game.Protobuf.Types.FightingRoundData> _repeated_roundData_codec
        = pb::FieldCodec.ForMessage(34, global::Game.Protobuf.Types.FightingRoundData.Parser);
    private readonly pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoundData> roundData_ = new pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoundData>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoundData> RoundData {
      get { return roundData_; }
    }

    /// <summary>Field number for the "Battle_Setting" field.</summary>
    public const int BattleSettingFieldNumber = 5;
    private global::Game.Protobuf.Types.FightingRoomBattleSetting battleSetting_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomBattleSetting BattleSetting {
      get { return battleSetting_; }
      set {
        battleSetting_ = value;
      }
    }

    /// <summary>Field number for the "Battle_Game_State" field.</summary>
    public const int BattleGameStateFieldNumber = 6;
    private global::Game.Protobuf.Types.FightingRoomBattleGameState battleGameState_ = global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomBattleGameState BattleGameState {
      get { return battleGameState_; }
      set {
        battleGameState_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRoomStageStateData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRoomStageStateData>.Equals(FightingRoomStageStateData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (IsPlanning != other.IsPlanning) return false;
      if (StageId != other.StageId) return false;
      if (CurrentRound != other.CurrentRound) return false;
      if(!roundData_.Equals(other.roundData_)) return false;
      if (!object.Equals(BattleSetting, other.BattleSetting)) return false;
      if (BattleGameState != other.BattleGameState) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (IsPlanning != 0) hash ^= IsPlanning.GetHashCode();
      if (StageId != 0) hash ^= StageId.GetHashCode();
      if (CurrentRound != 0) hash ^= CurrentRound.GetHashCode();
      hash ^= roundData_.GetHashCode();
      if (battleSetting_ != null) hash ^= BattleSetting.GetHashCode();
      if (BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) hash ^= BattleGameState.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (IsPlanning != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(IsPlanning);
      }
      if (StageId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(StageId);
      }
      if (CurrentRound != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(CurrentRound);
      }
      roundData_.WriteTo(output, _repeated_roundData_codec);
      if (battleSetting_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(BattleSetting);
      }
      if (BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) {
        output.WriteRawTag(48);
        output.WriteEnum((int) BattleGameState);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (IsPlanning != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(IsPlanning);
      }
      if (StageId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(StageId);
      }
      if (CurrentRound != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(CurrentRound);
      }
      roundData_.WriteTo(ref output, _repeated_roundData_codec);
      if (battleSetting_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(BattleSetting);
      }
      if (BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) {
        output.WriteRawTag(48);
        output.WriteEnum((int) BattleGameState);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (IsPlanning != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(IsPlanning);
      }
      if (StageId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(StageId);
      }
      if (CurrentRound != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(CurrentRound);
      }
      size += roundData_.CalculateSize(_repeated_roundData_codec);
      if (battleSetting_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(BattleSetting);
      }
      if (BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) BattleGameState);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRoomStageStateData>.MergeFrom(FightingRoomStageStateData other) {
      if (other == null) {
        return;
      }
      if (other.IsPlanning != 0) {
        IsPlanning = other.IsPlanning;
      }
      if (other.StageId != 0) {
        StageId = other.StageId;
      }
      if (other.CurrentRound != 0) {
        CurrentRound = other.CurrentRound;
      }
      roundData_.Add(other.roundData_);
      if (other.battleSetting_ != null) {
        if (battleSetting_ == null) {
          BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
        }
        BattleSetting.MergeFrom(other.BattleSetting);
      }
      if (other.BattleGameState != global::Game.Protobuf.Types.FightingRoomBattleGameState.Undefined) {
        BattleGameState = other.BattleGameState;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            IsPlanning = input.ReadInt32();
            break;
          }
          case 16: {
            StageId = input.ReadInt32();
            break;
          }
          case 24: {
            CurrentRound = input.ReadInt32();
            break;
          }
          case 34: {
            roundData_.AddEntriesFrom(input, _repeated_roundData_codec);
            break;
          }
          case 42: {
            if (battleSetting_ == null) {
              BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
            }
            input.ReadMessage(BattleSetting);
            break;
          }
          case 48: {
            BattleGameState = (global::Game.Protobuf.Types.FightingRoomBattleGameState) input.ReadEnum();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            IsPlanning = input.ReadInt32();
            break;
          }
          case 16: {
            StageId = input.ReadInt32();
            break;
          }
          case 24: {
            CurrentRound = input.ReadInt32();
            break;
          }
          case 34: {
            roundData_.AddEntriesFrom(ref input, _repeated_roundData_codec);
            break;
          }
          case 42: {
            if (battleSetting_ == null) {
              BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
            }
            input.ReadMessage(BattleSetting);
            break;
          }
          case 48: {
            BattleGameState = (global::Game.Protobuf.Types.FightingRoomBattleGameState) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// ==========PING_PONG==========
  /// </summary>
  public sealed partial class FightingRoomPingData : pb::IMessage<FightingRoomPingData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRoomPingData> _parser = new pb::MessageParser<FightingRoomPingData>(() => new FightingRoomPingData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRoomPingData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[7]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomPingData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomPingData(FightingRoomPingData other) : this() {
      id_ = other.id_;
      pingTime_ = other.pingTime_ != null ? other.pingTime_.Clone() : null;
      pongTime_ = other.pongTime_ != null ? other.pongTime_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRoomPingData pb::IDeepCloneable<FightingRoomPingData>.Clone() {
      return new FightingRoomPingData(this);
    }

    /// <summary>Field number for the "Id" field.</summary>
    public const int IdFieldNumber = 1;
    private int id_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "Ping_Time" field.</summary>
    public const int PingTimeFieldNumber = 2;
    private global::Google.Protobuf.WellKnownTypes.Timestamp pingTime_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Google.Protobuf.WellKnownTypes.Timestamp PingTime {
      get { return pingTime_; }
      set {
        pingTime_ = value;
      }
    }

    /// <summary>Field number for the "Pong_Time" field.</summary>
    public const int PongTimeFieldNumber = 3;
    private global::Google.Protobuf.WellKnownTypes.Timestamp pongTime_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Google.Protobuf.WellKnownTypes.Timestamp PongTime {
      get { return pongTime_; }
      set {
        pongTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRoomPingData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRoomPingData>.Equals(FightingRoomPingData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (!object.Equals(PingTime, other.PingTime)) return false;
      if (!object.Equals(PongTime, other.PongTime)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (pingTime_ != null) hash ^= PingTime.GetHashCode();
      if (pongTime_ != null) hash ^= PongTime.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Id);
      }
      if (pingTime_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(PingTime);
      }
      if (pongTime_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(PongTime);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Id);
      }
      if (pingTime_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(PingTime);
      }
      if (pongTime_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(PongTime);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (pingTime_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(PingTime);
      }
      if (pongTime_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(PongTime);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRoomPingData>.MergeFrom(FightingRoomPingData other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.pingTime_ != null) {
        if (pingTime_ == null) {
          PingTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        PingTime.MergeFrom(other.PingTime);
      }
      if (other.pongTime_ != null) {
        if (pongTime_ == null) {
          PongTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        PongTime.MergeFrom(other.PongTime);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Id = input.ReadInt32();
            break;
          }
          case 18: {
            if (pingTime_ == null) {
              PingTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(PingTime);
            break;
          }
          case 26: {
            if (pongTime_ == null) {
              PongTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(PongTime);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Id = input.ReadInt32();
            break;
          }
          case 18: {
            if (pingTime_ == null) {
              PingTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(PingTime);
            break;
          }
          case 26: {
            if (pongTime_ == null) {
              PongTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(PongTime);
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// ==========NET_USER_SYNC==========
  /// </summary>
  public sealed partial class SignalCharacterSync : pb::IMessage<SignalCharacterSync>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SignalCharacterSync> _parser = new pb::MessageParser<SignalCharacterSync>(() => new SignalCharacterSync());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<SignalCharacterSync> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[8]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SignalCharacterSync() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SignalCharacterSync(SignalCharacterSync other) : this() {
      syncFlag_ = other.syncFlag_;
      showName_ = other.showName_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    SignalCharacterSync pb::IDeepCloneable<SignalCharacterSync>.Clone() {
      return new SignalCharacterSync(this);
    }

    /// <summary>Field number for the "Sync_Flag" field.</summary>
    public const int SyncFlagFieldNumber = 1;
    private uint syncFlag_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint SyncFlag {
      get { return syncFlag_; }
      set {
        syncFlag_ = value;
      }
    }

    /// <summary>Field number for the "Show_Name" field.</summary>
    public const int ShowNameFieldNumber = 2;
    private string showName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string ShowName {
      get { return showName_; }
      set {
        showName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<SignalCharacterSync> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<SignalCharacterSync>.Equals(SignalCharacterSync other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SyncFlag != other.SyncFlag) return false;
      if (ShowName != other.ShowName) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (SyncFlag != 0) hash ^= SyncFlag.GetHashCode();
      if (ShowName.Length != 0) hash ^= ShowName.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (SyncFlag != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(SyncFlag);
      }
      if (ShowName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(ShowName);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (SyncFlag != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(SyncFlag);
      }
      if (ShowName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(ShowName);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (SyncFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(SyncFlag);
      }
      if (ShowName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ShowName);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<SignalCharacterSync>.MergeFrom(SignalCharacterSync other) {
      if (other == null) {
        return;
      }
      if (other.SyncFlag != 0) {
        SyncFlag = other.SyncFlag;
      }
      if (other.ShowName.Length != 0) {
        ShowName = other.ShowName;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            SyncFlag = input.ReadUInt32();
            break;
          }
          case 18: {
            ShowName = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            SyncFlag = input.ReadUInt32();
            break;
          }
          case 18: {
            ShowName = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class CommandCharacterSync : pb::IMessage<CommandCharacterSync>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CommandCharacterSync> _parser = new pb::MessageParser<CommandCharacterSync>(() => new CommandCharacterSync());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CommandCharacterSync> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[9]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CommandCharacterSync() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CommandCharacterSync(CommandCharacterSync other) : this() {
      isYou_ = other.isYou_;
      characterData_ = other.characterData_ != null ? other.characterData_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    CommandCharacterSync pb::IDeepCloneable<CommandCharacterSync>.Clone() {
      return new CommandCharacterSync(this);
    }

    /// <summary>Field number for the "Is_You" field.</summary>
    public const int IsYouFieldNumber = 1;
    private int isYou_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int IsYou {
      get { return isYou_; }
      set {
        isYou_ = value;
      }
    }

    /// <summary>Field number for the "Character_Data" field.</summary>
    public const int CharacterDataFieldNumber = 2;
    private global::Game.Protobuf.Types.CharacterData characterData_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.CharacterData CharacterData {
      get { return characterData_; }
      set {
        characterData_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<CommandCharacterSync> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<CommandCharacterSync>.Equals(CommandCharacterSync other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (IsYou != other.IsYou) return false;
      if (!object.Equals(CharacterData, other.CharacterData)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (IsYou != 0) hash ^= IsYou.GetHashCode();
      if (characterData_ != null) hash ^= CharacterData.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (IsYou != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(IsYou);
      }
      if (characterData_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(CharacterData);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (IsYou != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(IsYou);
      }
      if (characterData_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(CharacterData);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (IsYou != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(IsYou);
      }
      if (characterData_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(CharacterData);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<CommandCharacterSync>.MergeFrom(CommandCharacterSync other) {
      if (other == null) {
        return;
      }
      if (other.IsYou != 0) {
        IsYou = other.IsYou;
      }
      if (other.characterData_ != null) {
        if (characterData_ == null) {
          CharacterData = new global::Game.Protobuf.Types.CharacterData();
        }
        CharacterData.MergeFrom(other.CharacterData);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            IsYou = input.ReadInt32();
            break;
          }
          case 18: {
            if (characterData_ == null) {
              CharacterData = new global::Game.Protobuf.Types.CharacterData();
            }
            input.ReadMessage(CharacterData);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            IsYou = input.ReadInt32();
            break;
          }
          case 18: {
            if (characterData_ == null) {
              CharacterData = new global::Game.Protobuf.Types.CharacterData();
            }
            input.ReadMessage(CharacterData);
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// ==========CREATE_ROOM==========
  /// </summary>
  public sealed partial class RequestCreateFightingRoom : pb::IMessage<RequestCreateFightingRoom>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RequestCreateFightingRoom> _parser = new pb::MessageParser<RequestCreateFightingRoom>(() => new RequestCreateFightingRoom());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<RequestCreateFightingRoom> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[10]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestCreateFightingRoom() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestCreateFightingRoom(RequestCreateFightingRoom other) : this() {
      roomKey_ = other.roomKey_;
      roomSyncType_ = other.roomSyncType_;
      battleSetting_ = other.battleSetting_ != null ? other.battleSetting_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    RequestCreateFightingRoom pb::IDeepCloneable<RequestCreateFightingRoom>.Clone() {
      return new RequestCreateFightingRoom(this);
    }

    /// <summary>Field number for the "Room_Key" field.</summary>
    public const int RoomKeyFieldNumber = 1;
    private string roomKey_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string RoomKey {
      get { return roomKey_; }
      set {
        roomKey_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Room_Sync_Type" field.</summary>
    public const int RoomSyncTypeFieldNumber = 2;
    private global::Game.Protobuf.Types.FightingRoomSyncType roomSyncType_ = global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomSyncType RoomSyncType {
      get { return roomSyncType_; }
      set {
        roomSyncType_ = value;
      }
    }

    /// <summary>Field number for the "Battle_Setting" field.</summary>
    public const int BattleSettingFieldNumber = 3;
    private global::Game.Protobuf.Types.FightingRoomBattleSetting battleSetting_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomBattleSetting BattleSetting {
      get { return battleSetting_; }
      set {
        battleSetting_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<RequestCreateFightingRoom> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<RequestCreateFightingRoom>.Equals(RequestCreateFightingRoom other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RoomKey != other.RoomKey) return false;
      if (RoomSyncType != other.RoomSyncType) return false;
      if (!object.Equals(BattleSetting, other.BattleSetting)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (RoomKey.Length != 0) hash ^= RoomKey.GetHashCode();
      if (RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) hash ^= RoomSyncType.GetHashCode();
      if (battleSetting_ != null) hash ^= BattleSetting.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (RoomKey.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(RoomKey);
      }
      if (RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        output.WriteRawTag(16);
        output.WriteEnum((int) RoomSyncType);
      }
      if (battleSetting_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(BattleSetting);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (RoomKey.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(RoomKey);
      }
      if (RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        output.WriteRawTag(16);
        output.WriteEnum((int) RoomSyncType);
      }
      if (battleSetting_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(BattleSetting);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (RoomKey.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RoomKey);
      }
      if (RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) RoomSyncType);
      }
      if (battleSetting_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(BattleSetting);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<RequestCreateFightingRoom>.MergeFrom(RequestCreateFightingRoom other) {
      if (other == null) {
        return;
      }
      if (other.RoomKey.Length != 0) {
        RoomKey = other.RoomKey;
      }
      if (other.RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        RoomSyncType = other.RoomSyncType;
      }
      if (other.battleSetting_ != null) {
        if (battleSetting_ == null) {
          BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
        }
        BattleSetting.MergeFrom(other.BattleSetting);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            RoomKey = input.ReadString();
            break;
          }
          case 16: {
            RoomSyncType = (global::Game.Protobuf.Types.FightingRoomSyncType) input.ReadEnum();
            break;
          }
          case 26: {
            if (battleSetting_ == null) {
              BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
            }
            input.ReadMessage(BattleSetting);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            RoomKey = input.ReadString();
            break;
          }
          case 16: {
            RoomSyncType = (global::Game.Protobuf.Types.FightingRoomSyncType) input.ReadEnum();
            break;
          }
          case 26: {
            if (battleSetting_ == null) {
              BattleSetting = new global::Game.Protobuf.Types.FightingRoomBattleSetting();
            }
            input.ReadMessage(BattleSetting);
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// ==========JOIN_ROOM==========
  /// </summary>
  public sealed partial class RequestJoinFightingRoom : pb::IMessage<RequestJoinFightingRoom>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RequestJoinFightingRoom> _parser = new pb::MessageParser<RequestJoinFightingRoom>(() => new RequestJoinFightingRoom());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<RequestJoinFightingRoom> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[11]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestJoinFightingRoom() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestJoinFightingRoom(RequestJoinFightingRoom other) : this() {
      roomId_ = other.roomId_;
      roomKey_ = other.roomKey_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    RequestJoinFightingRoom pb::IDeepCloneable<RequestJoinFightingRoom>.Clone() {
      return new RequestJoinFightingRoom(this);
    }

    /// <summary>Field number for the "Room_Id" field.</summary>
    public const int RoomIdFieldNumber = 1;
    private string roomId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string RoomId {
      get { return roomId_; }
      set {
        roomId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Room_Key" field.</summary>
    public const int RoomKeyFieldNumber = 2;
    private string roomKey_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string RoomKey {
      get { return roomKey_; }
      set {
        roomKey_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<RequestJoinFightingRoom> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<RequestJoinFightingRoom>.Equals(RequestJoinFightingRoom other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RoomId != other.RoomId) return false;
      if (RoomKey != other.RoomKey) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (RoomId.Length != 0) hash ^= RoomId.GetHashCode();
      if (RoomKey.Length != 0) hash ^= RoomKey.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (RoomId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(RoomId);
      }
      if (RoomKey.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(RoomKey);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (RoomId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(RoomId);
      }
      if (RoomKey.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(RoomKey);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (RoomId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RoomId);
      }
      if (RoomKey.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RoomKey);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<RequestJoinFightingRoom>.MergeFrom(RequestJoinFightingRoom other) {
      if (other == null) {
        return;
      }
      if (other.RoomId.Length != 0) {
        RoomId = other.RoomId;
      }
      if (other.RoomKey.Length != 0) {
        RoomKey = other.RoomKey;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            RoomId = input.ReadString();
            break;
          }
          case 18: {
            RoomKey = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            RoomId = input.ReadString();
            break;
          }
          case 18: {
            RoomKey = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// ==========LEAVE_ROOM==========
  /// </summary>
  public sealed partial class RequestLeaveFightingRoom : pb::IMessage<RequestLeaveFightingRoom>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RequestLeaveFightingRoom> _parser = new pb::MessageParser<RequestLeaveFightingRoom>(() => new RequestLeaveFightingRoom());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<RequestLeaveFightingRoom> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[12]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestLeaveFightingRoom() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestLeaveFightingRoom(RequestLeaveFightingRoom other) : this() {
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    RequestLeaveFightingRoom pb::IDeepCloneable<RequestLeaveFightingRoom>.Clone() {
      return new RequestLeaveFightingRoom(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<RequestLeaveFightingRoom> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<RequestLeaveFightingRoom>.Equals(RequestLeaveFightingRoom other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<RequestLeaveFightingRoom>.MergeFrom(RequestLeaveFightingRoom other) {
      if (other == null) {
        return;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
        }
      }
    }
    #endif

  }

  /// <summary>
  /// ==========ROOM_SYNC==========
  /// </summary>
  public sealed partial class CommandFightingRoomSync : pb::IMessage<CommandFightingRoomSync>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CommandFightingRoomSync> _parser = new pb::MessageParser<CommandFightingRoomSync>(() => new CommandFightingRoomSync());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CommandFightingRoomSync> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[13]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CommandFightingRoomSync() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CommandFightingRoomSync(CommandFightingRoomSync other) : this() {
      roomId_ = other.roomId_;
      roomMasterCharacterId_ = other.roomMasterCharacterId_;
      roomSyncType_ = other.roomSyncType_;
      stageStateData_ = other.stageStateData_ != null ? other.stageStateData_.Clone() : null;
      characterStateDatas_ = other.characterStateDatas_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    CommandFightingRoomSync pb::IDeepCloneable<CommandFightingRoomSync>.Clone() {
      return new CommandFightingRoomSync(this);
    }

    /// <summary>Field number for the "Room_Id" field.</summary>
    public const int RoomIdFieldNumber = 1;
    private string roomId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string RoomId {
      get { return roomId_; }
      set {
        roomId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Room_Master_Character_Id" field.</summary>
    public const int RoomMasterCharacterIdFieldNumber = 2;
    private ulong roomMasterCharacterId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ulong RoomMasterCharacterId {
      get { return roomMasterCharacterId_; }
      set {
        roomMasterCharacterId_ = value;
      }
    }

    /// <summary>Field number for the "Room_Sync_Type" field.</summary>
    public const int RoomSyncTypeFieldNumber = 3;
    private global::Game.Protobuf.Types.FightingRoomSyncType roomSyncType_ = global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep;
    /// <summary>
    /// 同步類型 鎖幀或OCC
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomSyncType RoomSyncType {
      get { return roomSyncType_; }
      set {
        roomSyncType_ = value;
      }
    }

    /// <summary>Field number for the "Stage_State_Data" field.</summary>
    public const int StageStateDataFieldNumber = 4;
    private global::Game.Protobuf.Types.FightingRoomStageStateData stageStateData_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomStageStateData StageStateData {
      get { return stageStateData_; }
      set {
        stageStateData_ = value;
      }
    }

    /// <summary>Field number for the "Character_State_Datas" field.</summary>
    public const int CharacterStateDatasFieldNumber = 5;
    private static readonly pb::FieldCodec<global::Game.Protobuf.Types.FightingRoomCharacterStateData> _repeated_characterStateDatas_codec
        = pb::FieldCodec.ForMessage(42, global::Game.Protobuf.Types.FightingRoomCharacterStateData.Parser);
    private readonly pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoomCharacterStateData> characterStateDatas_ = new pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoomCharacterStateData>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Game.Protobuf.Types.FightingRoomCharacterStateData> CharacterStateDatas {
      get { return characterStateDatas_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<CommandFightingRoomSync> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<CommandFightingRoomSync>.Equals(CommandFightingRoomSync other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RoomId != other.RoomId) return false;
      if (RoomMasterCharacterId != other.RoomMasterCharacterId) return false;
      if (RoomSyncType != other.RoomSyncType) return false;
      if (!object.Equals(StageStateData, other.StageStateData)) return false;
      if(!characterStateDatas_.Equals(other.characterStateDatas_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (RoomId.Length != 0) hash ^= RoomId.GetHashCode();
      if (RoomMasterCharacterId != 0UL) hash ^= RoomMasterCharacterId.GetHashCode();
      if (RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) hash ^= RoomSyncType.GetHashCode();
      if (stageStateData_ != null) hash ^= StageStateData.GetHashCode();
      hash ^= characterStateDatas_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (RoomId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(RoomId);
      }
      if (RoomMasterCharacterId != 0UL) {
        output.WriteRawTag(16);
        output.WriteUInt64(RoomMasterCharacterId);
      }
      if (RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        output.WriteRawTag(24);
        output.WriteEnum((int) RoomSyncType);
      }
      if (stageStateData_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(StageStateData);
      }
      characterStateDatas_.WriteTo(output, _repeated_characterStateDatas_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (RoomId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(RoomId);
      }
      if (RoomMasterCharacterId != 0UL) {
        output.WriteRawTag(16);
        output.WriteUInt64(RoomMasterCharacterId);
      }
      if (RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        output.WriteRawTag(24);
        output.WriteEnum((int) RoomSyncType);
      }
      if (stageStateData_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(StageStateData);
      }
      characterStateDatas_.WriteTo(ref output, _repeated_characterStateDatas_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (RoomId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RoomId);
      }
      if (RoomMasterCharacterId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(RoomMasterCharacterId);
      }
      if (RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) RoomSyncType);
      }
      if (stageStateData_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(StageStateData);
      }
      size += characterStateDatas_.CalculateSize(_repeated_characterStateDatas_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<CommandFightingRoomSync>.MergeFrom(CommandFightingRoomSync other) {
      if (other == null) {
        return;
      }
      if (other.RoomId.Length != 0) {
        RoomId = other.RoomId;
      }
      if (other.RoomMasterCharacterId != 0UL) {
        RoomMasterCharacterId = other.RoomMasterCharacterId;
      }
      if (other.RoomSyncType != global::Game.Protobuf.Types.FightingRoomSyncType.Lockstep) {
        RoomSyncType = other.RoomSyncType;
      }
      if (other.stageStateData_ != null) {
        if (stageStateData_ == null) {
          StageStateData = new global::Game.Protobuf.Types.FightingRoomStageStateData();
        }
        StageStateData.MergeFrom(other.StageStateData);
      }
      characterStateDatas_.Add(other.characterStateDatas_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            RoomId = input.ReadString();
            break;
          }
          case 16: {
            RoomMasterCharacterId = input.ReadUInt64();
            break;
          }
          case 24: {
            RoomSyncType = (global::Game.Protobuf.Types.FightingRoomSyncType) input.ReadEnum();
            break;
          }
          case 34: {
            if (stageStateData_ == null) {
              StageStateData = new global::Game.Protobuf.Types.FightingRoomStageStateData();
            }
            input.ReadMessage(StageStateData);
            break;
          }
          case 42: {
            characterStateDatas_.AddEntriesFrom(input, _repeated_characterStateDatas_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            RoomId = input.ReadString();
            break;
          }
          case 16: {
            RoomMasterCharacterId = input.ReadUInt64();
            break;
          }
          case 24: {
            RoomSyncType = (global::Game.Protobuf.Types.FightingRoomSyncType) input.ReadEnum();
            break;
          }
          case 34: {
            if (stageStateData_ == null) {
              StageStateData = new global::Game.Protobuf.Types.FightingRoomStageStateData();
            }
            input.ReadMessage(StageStateData);
            break;
          }
          case 42: {
            characterStateDatas_.AddEntriesFrom(ref input, _repeated_characterStateDatas_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// ==========ROOM_ACTION==========
  /// Server To Client
  /// </summary>
  public sealed partial class CommandFightingRoomAction : pb::IMessage<CommandFightingRoomAction>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CommandFightingRoomAction> _parser = new pb::MessageParser<CommandFightingRoomAction>(() => new CommandFightingRoomAction());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CommandFightingRoomAction> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[14]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CommandFightingRoomAction() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CommandFightingRoomAction(CommandFightingRoomAction other) : this() {
      actionType_ = other.actionType_;
      characterId_ = other.characterId_;
      syncIntId_ = other.syncIntId_;
      syncPosition_ = other.syncPosition_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    CommandFightingRoomAction pb::IDeepCloneable<CommandFightingRoomAction>.Clone() {
      return new CommandFightingRoomAction(this);
    }

    /// <summary>Field number for the "Action_Type" field.</summary>
    public const int ActionTypeFieldNumber = 1;
    private global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType actionType_ = global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType.Undefined;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType ActionType {
      get { return actionType_; }
      set {
        actionType_ = value;
      }
    }

    /// <summary>Field number for the "Character_Id" field.</summary>
    public const int CharacterIdFieldNumber = 2;
    private ulong characterId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ulong CharacterId {
      get { return characterId_; }
      set {
        characterId_ = value;
      }
    }

    /// <summary>Field number for the "Sync_Int_Id" field.</summary>
    public const int SyncIntIdFieldNumber = 3;
    private int syncIntId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SyncIntId {
      get { return syncIntId_; }
      set {
        syncIntId_ = value;
      }
    }

    /// <summary>Field number for the "Sync_Position" field.</summary>
    public const int SyncPositionFieldNumber = 4;
    private global::Game.Protobuf.Types.FightingRoomPlayerPosition syncPosition_ = global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomPlayerPosition SyncPosition {
      get { return syncPosition_; }
      set {
        syncPosition_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<CommandFightingRoomAction> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<CommandFightingRoomAction>.Equals(CommandFightingRoomAction other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ActionType != other.ActionType) return false;
      if (CharacterId != other.CharacterId) return false;
      if (SyncIntId != other.SyncIntId) return false;
      if (SyncPosition != other.SyncPosition) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ActionType != global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType.Undefined) hash ^= ActionType.GetHashCode();
      if (CharacterId != 0UL) hash ^= CharacterId.GetHashCode();
      if (SyncIntId != 0) hash ^= SyncIntId.GetHashCode();
      if (SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) hash ^= SyncPosition.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (ActionType != global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType.Undefined) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ActionType);
      }
      if (CharacterId != 0UL) {
        output.WriteRawTag(16);
        output.WriteUInt64(CharacterId);
      }
      if (SyncIntId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(SyncIntId);
      }
      if (SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        output.WriteRawTag(32);
        output.WriteEnum((int) SyncPosition);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ActionType != global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType.Undefined) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ActionType);
      }
      if (CharacterId != 0UL) {
        output.WriteRawTag(16);
        output.WriteUInt64(CharacterId);
      }
      if (SyncIntId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(SyncIntId);
      }
      if (SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        output.WriteRawTag(32);
        output.WriteEnum((int) SyncPosition);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (ActionType != global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType.Undefined) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ActionType);
      }
      if (CharacterId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(CharacterId);
      }
      if (SyncIntId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SyncIntId);
      }
      if (SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) SyncPosition);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<CommandFightingRoomAction>.MergeFrom(CommandFightingRoomAction other) {
      if (other == null) {
        return;
      }
      if (other.ActionType != global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType.Undefined) {
        ActionType = other.ActionType;
      }
      if (other.CharacterId != 0UL) {
        CharacterId = other.CharacterId;
      }
      if (other.SyncIntId != 0) {
        SyncIntId = other.SyncIntId;
      }
      if (other.SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        SyncPosition = other.SyncPosition;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            ActionType = (global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType) input.ReadEnum();
            break;
          }
          case 16: {
            CharacterId = input.ReadUInt64();
            break;
          }
          case 24: {
            SyncIntId = input.ReadInt32();
            break;
          }
          case 32: {
            SyncPosition = (global::Game.Protobuf.Types.FightingRoomPlayerPosition) input.ReadEnum();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            ActionType = (global::Game.Protobuf.Types.CommandFightingRoomAction.Types.CommandActionType) input.ReadEnum();
            break;
          }
          case 16: {
            CharacterId = input.ReadUInt64();
            break;
          }
          case 24: {
            SyncIntId = input.ReadInt32();
            break;
          }
          case 32: {
            SyncPosition = (global::Game.Protobuf.Types.FightingRoomPlayerPosition) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the CommandFightingRoomAction message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Types {
      public enum CommandActionType {
        [pbr::OriginalName("Undefined")] Undefined = 0,
        [pbr::OriginalName("Room_Enter")] RoomEnter = 1,
        [pbr::OriginalName("Room_Leave")] RoomLeave = 2,
        [pbr::OriginalName("Plan_Ready_On")] PlanReadyOn = 3,
        [pbr::OriginalName("Plan_Ready_Off")] PlanReadyOff = 4,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Role_Id")] RoleId = 6,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Weapon_Id")] WeaponId = 7,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Stage_Id")] StageId = 8,
        /// <summary>
        /// use sync_position
        /// </summary>
        [pbr::OriginalName("Change_Position")] ChangePosition = 9,
        [pbr::OriginalName("Goto_Battle_Stage")] GotoBattleStage = 10,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Play_Again_Type")] PlayAgainType = 11,
        [pbr::OriginalName("Skip_Animation")] SkipAnimation = 12,
      }

    }
    #endregion

  }

  /// <summary>
  /// Client To Server
  /// </summary>
  public sealed partial class SignalFightingRoomAction : pb::IMessage<SignalFightingRoomAction>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SignalFightingRoomAction> _parser = new pb::MessageParser<SignalFightingRoomAction>(() => new SignalFightingRoomAction());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<SignalFightingRoomAction> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[15]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SignalFightingRoomAction() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SignalFightingRoomAction(SignalFightingRoomAction other) : this() {
      actionType_ = other.actionType_;
      syncIntId_ = other.syncIntId_;
      syncPosition_ = other.syncPosition_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    SignalFightingRoomAction pb::IDeepCloneable<SignalFightingRoomAction>.Clone() {
      return new SignalFightingRoomAction(this);
    }

    /// <summary>Field number for the "Action_Type" field.</summary>
    public const int ActionTypeFieldNumber = 1;
    private global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType actionType_ = global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType.Undefined;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType ActionType {
      get { return actionType_; }
      set {
        actionType_ = value;
      }
    }

    /// <summary>Field number for the "Sync_Int_Id" field.</summary>
    public const int SyncIntIdFieldNumber = 2;
    private int syncIntId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SyncIntId {
      get { return syncIntId_; }
      set {
        syncIntId_ = value;
      }
    }

    /// <summary>Field number for the "Sync_Position" field.</summary>
    public const int SyncPositionFieldNumber = 3;
    private global::Game.Protobuf.Types.FightingRoomPlayerPosition syncPosition_ = global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.FightingRoomPlayerPosition SyncPosition {
      get { return syncPosition_; }
      set {
        syncPosition_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<SignalFightingRoomAction> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<SignalFightingRoomAction>.Equals(SignalFightingRoomAction other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ActionType != other.ActionType) return false;
      if (SyncIntId != other.SyncIntId) return false;
      if (SyncPosition != other.SyncPosition) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ActionType != global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType.Undefined) hash ^= ActionType.GetHashCode();
      if (SyncIntId != 0) hash ^= SyncIntId.GetHashCode();
      if (SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) hash ^= SyncPosition.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (ActionType != global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType.Undefined) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ActionType);
      }
      if (SyncIntId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(SyncIntId);
      }
      if (SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        output.WriteRawTag(24);
        output.WriteEnum((int) SyncPosition);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ActionType != global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType.Undefined) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ActionType);
      }
      if (SyncIntId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(SyncIntId);
      }
      if (SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        output.WriteRawTag(24);
        output.WriteEnum((int) SyncPosition);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (ActionType != global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType.Undefined) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ActionType);
      }
      if (SyncIntId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SyncIntId);
      }
      if (SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) SyncPosition);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<SignalFightingRoomAction>.MergeFrom(SignalFightingRoomAction other) {
      if (other == null) {
        return;
      }
      if (other.ActionType != global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType.Undefined) {
        ActionType = other.ActionType;
      }
      if (other.SyncIntId != 0) {
        SyncIntId = other.SyncIntId;
      }
      if (other.SyncPosition != global::Game.Protobuf.Types.FightingRoomPlayerPosition.NoPosition) {
        SyncPosition = other.SyncPosition;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            ActionType = (global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType) input.ReadEnum();
            break;
          }
          case 16: {
            SyncIntId = input.ReadInt32();
            break;
          }
          case 24: {
            SyncPosition = (global::Game.Protobuf.Types.FightingRoomPlayerPosition) input.ReadEnum();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            ActionType = (global::Game.Protobuf.Types.SignalFightingRoomAction.Types.SignalActionType) input.ReadEnum();
            break;
          }
          case 16: {
            SyncIntId = input.ReadInt32();
            break;
          }
          case 24: {
            SyncPosition = (global::Game.Protobuf.Types.FightingRoomPlayerPosition) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the SignalFightingRoomAction message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Types {
      public enum SignalActionType {
        [pbr::OriginalName("Undefined")] Undefined = 0,
        [pbr::OriginalName("Plan_Ready_On")] PlanReadyOn = 1,
        [pbr::OriginalName("Plan_Ready_Off")] PlanReadyOff = 2,
        [pbr::OriginalName("Plan_Start_Game")] PlanStartGame = 3,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Role_Id")] RoleId = 4,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Weapon_Id")] WeaponId = 5,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Stage_Id")] StageId = 6,
        /// <summary>
        /// use sync_position
        /// </summary>
        [pbr::OriginalName("Change_Position")] ChangePosition = 7,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Set_Input_Delay_Frame")] SetInputDelayFrame = 8,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Battle_Game_State")] BattleGameState = 9,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Set_Game_Bo")] SetGameBo = 10,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Set_Round_Time")] SetRoundTime = 11,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Set_Input_Fps")] SetInputFps = 12,
        /// <summary>
        /// use sync_int_id
        /// </summary>
        [pbr::OriginalName("Play_Again_Type")] PlayAgainType = 13,
        [pbr::OriginalName("Skip_Animation")] SkipAnimation = 14,
      }

    }
    #endregion

  }

  /// <summary>
  /// ==========BATTLE_KEY_DOWN_SYNC==========
  /// </summary>
  public sealed partial class FightingRooomBattleKeyDownSync : pb::IMessage<FightingRooomBattleKeyDownSync>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRooomBattleKeyDownSync> _parser = new pb::MessageParser<FightingRooomBattleKeyDownSync>(() => new FightingRooomBattleKeyDownSync());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRooomBattleKeyDownSync> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[16]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRooomBattleKeyDownSync() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRooomBattleKeyDownSync(FightingRooomBattleKeyDownSync other) : this() {
      round_ = other.round_;
      playerId_ = other.playerId_;
      flagFromFrame_ = other.flagFromFrame_;
      keyDownFlag_ = other.keyDownFlag_.Clone();
      lastReceiveAdvanceFrameX100_ = other.lastReceiveAdvanceFrameX100_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRooomBattleKeyDownSync pb::IDeepCloneable<FightingRooomBattleKeyDownSync>.Clone() {
      return new FightingRooomBattleKeyDownSync(this);
    }

    /// <summary>Field number for the "Round" field.</summary>
    public const int RoundFieldNumber = 1;
    private int round_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Round {
      get { return round_; }
      set {
        round_ = value;
      }
    }

    /// <summary>Field number for the "Player_Id" field.</summary>
    public const int PlayerIdFieldNumber = 2;
    private int playerId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int PlayerId {
      get { return playerId_; }
      set {
        playerId_ = value;
      }
    }

    /// <summary>Field number for the "Flag_From_Frame" field.</summary>
    public const int FlagFromFrameFieldNumber = 3;
    private int flagFromFrame_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int FlagFromFrame {
      get { return flagFromFrame_; }
      set {
        flagFromFrame_ = value;
      }
    }

    /// <summary>Field number for the "Key_Down_Flag" field.</summary>
    public const int KeyDownFlagFieldNumber = 4;
    private static readonly pb::FieldCodec<uint> _repeated_keyDownFlag_codec
        = pb::FieldCodec.ForUInt32(34);
    private readonly pbc::RepeatedField<uint> keyDownFlag_ = new pbc::RepeatedField<uint>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<uint> KeyDownFlag {
      get { return keyDownFlag_; }
    }

    /// <summary>Field number for the "Last_Receive_Advance_Frame_X100" field.</summary>
    public const int LastReceiveAdvanceFrameX100FieldNumber = 5;
    private int lastReceiveAdvanceFrameX100_;
    /// <summary>
    /// 最近一次收到指令時, 續離執行該指令還差多少Frame(+早到, -晚到)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int LastReceiveAdvanceFrameX100 {
      get { return lastReceiveAdvanceFrameX100_; }
      set {
        lastReceiveAdvanceFrameX100_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRooomBattleKeyDownSync> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRooomBattleKeyDownSync>.Equals(FightingRooomBattleKeyDownSync other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Round != other.Round) return false;
      if (PlayerId != other.PlayerId) return false;
      if (FlagFromFrame != other.FlagFromFrame) return false;
      if(!keyDownFlag_.Equals(other.keyDownFlag_)) return false;
      if (LastReceiveAdvanceFrameX100 != other.LastReceiveAdvanceFrameX100) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Round != 0) hash ^= Round.GetHashCode();
      if (PlayerId != 0) hash ^= PlayerId.GetHashCode();
      if (FlagFromFrame != 0) hash ^= FlagFromFrame.GetHashCode();
      hash ^= keyDownFlag_.GetHashCode();
      if (LastReceiveAdvanceFrameX100 != 0) hash ^= LastReceiveAdvanceFrameX100.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Round != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Round);
      }
      if (PlayerId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(PlayerId);
      }
      if (FlagFromFrame != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(FlagFromFrame);
      }
      keyDownFlag_.WriteTo(output, _repeated_keyDownFlag_codec);
      if (LastReceiveAdvanceFrameX100 != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(LastReceiveAdvanceFrameX100);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Round != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Round);
      }
      if (PlayerId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(PlayerId);
      }
      if (FlagFromFrame != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(FlagFromFrame);
      }
      keyDownFlag_.WriteTo(ref output, _repeated_keyDownFlag_codec);
      if (LastReceiveAdvanceFrameX100 != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(LastReceiveAdvanceFrameX100);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (Round != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Round);
      }
      if (PlayerId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(PlayerId);
      }
      if (FlagFromFrame != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(FlagFromFrame);
      }
      size += keyDownFlag_.CalculateSize(_repeated_keyDownFlag_codec);
      if (LastReceiveAdvanceFrameX100 != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(LastReceiveAdvanceFrameX100);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRooomBattleKeyDownSync>.MergeFrom(FightingRooomBattleKeyDownSync other) {
      if (other == null) {
        return;
      }
      if (other.Round != 0) {
        Round = other.Round;
      }
      if (other.PlayerId != 0) {
        PlayerId = other.PlayerId;
      }
      if (other.FlagFromFrame != 0) {
        FlagFromFrame = other.FlagFromFrame;
      }
      keyDownFlag_.Add(other.keyDownFlag_);
      if (other.LastReceiveAdvanceFrameX100 != 0) {
        LastReceiveAdvanceFrameX100 = other.LastReceiveAdvanceFrameX100;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Round = input.ReadInt32();
            break;
          }
          case 16: {
            PlayerId = input.ReadInt32();
            break;
          }
          case 24: {
            FlagFromFrame = input.ReadInt32();
            break;
          }
          case 34:
          case 32: {
            keyDownFlag_.AddEntriesFrom(input, _repeated_keyDownFlag_codec);
            break;
          }
          case 40: {
            LastReceiveAdvanceFrameX100 = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Round = input.ReadInt32();
            break;
          }
          case 16: {
            PlayerId = input.ReadInt32();
            break;
          }
          case 24: {
            FlagFromFrame = input.ReadInt32();
            break;
          }
          case 34:
          case 32: {
            keyDownFlag_.AddEntriesFrom(ref input, _repeated_keyDownFlag_codec);
            break;
          }
          case 40: {
            LastReceiveAdvanceFrameX100 = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// ==========SEND_BATTLE_RESULT==========
  /// </summary>
  public sealed partial class BattleActionDataLog : pb::IMessage<BattleActionDataLog>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<BattleActionDataLog> _parser = new pb::MessageParser<BattleActionDataLog>(() => new BattleActionDataLog());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<BattleActionDataLog> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[17]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattleActionDataLog() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattleActionDataLog(BattleActionDataLog other) : this() {
      roleId_ = other.roleId_;
      weaponId_ = other.weaponId_;
      subWeaponId_ = other.subWeaponId_;
      weaponBrokenTimes_ = other.weaponBrokenTimes_;
      subWeaponBrokenTimes_ = other.subWeaponBrokenTimes_;
      usedThrowTimes_ = other.usedThrowTimes_;
      usedDeThrowTimes_ = other.usedDeThrowTimes_;
      usedReversalTimes_ = other.usedReversalTimes_;
      characterId_ = other.characterId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    BattleActionDataLog pb::IDeepCloneable<BattleActionDataLog>.Clone() {
      return new BattleActionDataLog(this);
    }

    /// <summary>Field number for the "RoleId" field.</summary>
    public const int RoleIdFieldNumber = 1;
    private int roleId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int RoleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    /// <summary>Field number for the "WeaponId" field.</summary>
    public const int WeaponIdFieldNumber = 2;
    private int weaponId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int WeaponId {
      get { return weaponId_; }
      set {
        weaponId_ = value;
      }
    }

    /// <summary>Field number for the "SubWeaponId" field.</summary>
    public const int SubWeaponIdFieldNumber = 3;
    private int subWeaponId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SubWeaponId {
      get { return subWeaponId_; }
      set {
        subWeaponId_ = value;
      }
    }

    /// <summary>Field number for the "WeaponBrokenTimes" field.</summary>
    public const int WeaponBrokenTimesFieldNumber = 4;
    private int weaponBrokenTimes_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int WeaponBrokenTimes {
      get { return weaponBrokenTimes_; }
      set {
        weaponBrokenTimes_ = value;
      }
    }

    /// <summary>Field number for the "SubWeaponBrokenTimes" field.</summary>
    public const int SubWeaponBrokenTimesFieldNumber = 5;
    private int subWeaponBrokenTimes_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SubWeaponBrokenTimes {
      get { return subWeaponBrokenTimes_; }
      set {
        subWeaponBrokenTimes_ = value;
      }
    }

    /// <summary>Field number for the "UsedThrowTimes" field.</summary>
    public const int UsedThrowTimesFieldNumber = 6;
    private int usedThrowTimes_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int UsedThrowTimes {
      get { return usedThrowTimes_; }
      set {
        usedThrowTimes_ = value;
      }
    }

    /// <summary>Field number for the "UsedDeThrowTimes" field.</summary>
    public const int UsedDeThrowTimesFieldNumber = 7;
    private int usedDeThrowTimes_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int UsedDeThrowTimes {
      get { return usedDeThrowTimes_; }
      set {
        usedDeThrowTimes_ = value;
      }
    }

    /// <summary>Field number for the "UsedReversalTimes" field.</summary>
    public const int UsedReversalTimesFieldNumber = 8;
    private int usedReversalTimes_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int UsedReversalTimes {
      get { return usedReversalTimes_; }
      set {
        usedReversalTimes_ = value;
      }
    }

    /// <summary>Field number for the "CharacterId" field.</summary>
    public const int CharacterIdFieldNumber = 9;
    private uint characterId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint CharacterId {
      get { return characterId_; }
      set {
        characterId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<BattleActionDataLog> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<BattleActionDataLog>.Equals(BattleActionDataLog other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RoleId != other.RoleId) return false;
      if (WeaponId != other.WeaponId) return false;
      if (SubWeaponId != other.SubWeaponId) return false;
      if (WeaponBrokenTimes != other.WeaponBrokenTimes) return false;
      if (SubWeaponBrokenTimes != other.SubWeaponBrokenTimes) return false;
      if (UsedThrowTimes != other.UsedThrowTimes) return false;
      if (UsedDeThrowTimes != other.UsedDeThrowTimes) return false;
      if (UsedReversalTimes != other.UsedReversalTimes) return false;
      if (CharacterId != other.CharacterId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (RoleId != 0) hash ^= RoleId.GetHashCode();
      if (WeaponId != 0) hash ^= WeaponId.GetHashCode();
      if (SubWeaponId != 0) hash ^= SubWeaponId.GetHashCode();
      if (WeaponBrokenTimes != 0) hash ^= WeaponBrokenTimes.GetHashCode();
      if (SubWeaponBrokenTimes != 0) hash ^= SubWeaponBrokenTimes.GetHashCode();
      if (UsedThrowTimes != 0) hash ^= UsedThrowTimes.GetHashCode();
      if (UsedDeThrowTimes != 0) hash ^= UsedDeThrowTimes.GetHashCode();
      if (UsedReversalTimes != 0) hash ^= UsedReversalTimes.GetHashCode();
      if (CharacterId != 0) hash ^= CharacterId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (RoleId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(RoleId);
      }
      if (WeaponId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(WeaponId);
      }
      if (SubWeaponId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(SubWeaponId);
      }
      if (WeaponBrokenTimes != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(WeaponBrokenTimes);
      }
      if (SubWeaponBrokenTimes != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(SubWeaponBrokenTimes);
      }
      if (UsedThrowTimes != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(UsedThrowTimes);
      }
      if (UsedDeThrowTimes != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(UsedDeThrowTimes);
      }
      if (UsedReversalTimes != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(UsedReversalTimes);
      }
      if (CharacterId != 0) {
        output.WriteRawTag(72);
        output.WriteUInt32(CharacterId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (RoleId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(RoleId);
      }
      if (WeaponId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(WeaponId);
      }
      if (SubWeaponId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(SubWeaponId);
      }
      if (WeaponBrokenTimes != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(WeaponBrokenTimes);
      }
      if (SubWeaponBrokenTimes != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(SubWeaponBrokenTimes);
      }
      if (UsedThrowTimes != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(UsedThrowTimes);
      }
      if (UsedDeThrowTimes != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(UsedDeThrowTimes);
      }
      if (UsedReversalTimes != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(UsedReversalTimes);
      }
      if (CharacterId != 0) {
        output.WriteRawTag(72);
        output.WriteUInt32(CharacterId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (RoleId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoleId);
      }
      if (WeaponId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(WeaponId);
      }
      if (SubWeaponId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SubWeaponId);
      }
      if (WeaponBrokenTimes != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(WeaponBrokenTimes);
      }
      if (SubWeaponBrokenTimes != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SubWeaponBrokenTimes);
      }
      if (UsedThrowTimes != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(UsedThrowTimes);
      }
      if (UsedDeThrowTimes != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(UsedDeThrowTimes);
      }
      if (UsedReversalTimes != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(UsedReversalTimes);
      }
      if (CharacterId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(CharacterId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<BattleActionDataLog>.MergeFrom(BattleActionDataLog other) {
      if (other == null) {
        return;
      }
      if (other.RoleId != 0) {
        RoleId = other.RoleId;
      }
      if (other.WeaponId != 0) {
        WeaponId = other.WeaponId;
      }
      if (other.SubWeaponId != 0) {
        SubWeaponId = other.SubWeaponId;
      }
      if (other.WeaponBrokenTimes != 0) {
        WeaponBrokenTimes = other.WeaponBrokenTimes;
      }
      if (other.SubWeaponBrokenTimes != 0) {
        SubWeaponBrokenTimes = other.SubWeaponBrokenTimes;
      }
      if (other.UsedThrowTimes != 0) {
        UsedThrowTimes = other.UsedThrowTimes;
      }
      if (other.UsedDeThrowTimes != 0) {
        UsedDeThrowTimes = other.UsedDeThrowTimes;
      }
      if (other.UsedReversalTimes != 0) {
        UsedReversalTimes = other.UsedReversalTimes;
      }
      if (other.CharacterId != 0) {
        CharacterId = other.CharacterId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            RoleId = input.ReadInt32();
            break;
          }
          case 16: {
            WeaponId = input.ReadInt32();
            break;
          }
          case 24: {
            SubWeaponId = input.ReadInt32();
            break;
          }
          case 32: {
            WeaponBrokenTimes = input.ReadInt32();
            break;
          }
          case 40: {
            SubWeaponBrokenTimes = input.ReadInt32();
            break;
          }
          case 48: {
            UsedThrowTimes = input.ReadInt32();
            break;
          }
          case 56: {
            UsedDeThrowTimes = input.ReadInt32();
            break;
          }
          case 64: {
            UsedReversalTimes = input.ReadInt32();
            break;
          }
          case 72: {
            CharacterId = input.ReadUInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            RoleId = input.ReadInt32();
            break;
          }
          case 16: {
            WeaponId = input.ReadInt32();
            break;
          }
          case 24: {
            SubWeaponId = input.ReadInt32();
            break;
          }
          case 32: {
            WeaponBrokenTimes = input.ReadInt32();
            break;
          }
          case 40: {
            SubWeaponBrokenTimes = input.ReadInt32();
            break;
          }
          case 48: {
            UsedThrowTimes = input.ReadInt32();
            break;
          }
          case 56: {
            UsedDeThrowTimes = input.ReadInt32();
            break;
          }
          case 64: {
            UsedReversalTimes = input.ReadInt32();
            break;
          }
          case 72: {
            CharacterId = input.ReadUInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FightingRoomBattleResult : pb::IMessage<FightingRoomBattleResult>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRoomBattleResult> _parser = new pb::MessageParser<FightingRoomBattleResult>(() => new FightingRoomBattleResult());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRoomBattleResult> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[18]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomBattleResult() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomBattleResult(FightingRoomBattleResult other) : this() {
      errorCode_ = other.errorCode_;
      ticketPasscode_ = other.ticketPasscode_;
      result_ = other.result_;
      isSuddenDeath_ = other.isSuddenDeath_;
      battleAction_ = other.battleAction_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRoomBattleResult pb::IDeepCloneable<FightingRoomBattleResult>.Clone() {
      return new FightingRoomBattleResult(this);
    }

    /// <summary>Field number for the "ErrorCode" field.</summary>
    public const int ErrorCodeFieldNumber = 1;
    private int errorCode_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ErrorCode {
      get { return errorCode_; }
      set {
        errorCode_ = value;
      }
    }

    /// <summary>Field number for the "TicketPasscode" field.</summary>
    public const int TicketPasscodeFieldNumber = 2;
    private string ticketPasscode_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string TicketPasscode {
      get { return ticketPasscode_; }
      set {
        ticketPasscode_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Result" field.</summary>
    public const int ResultFieldNumber = 3;
    private global::Game.Protobuf.Types.BattleResult result_ = global::Game.Protobuf.Types.BattleResult.ResultError;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.BattleResult Result {
      get { return result_; }
      set {
        result_ = value;
      }
    }

    /// <summary>Field number for the "IsSuddenDeath" field.</summary>
    public const int IsSuddenDeathFieldNumber = 4;
    private int isSuddenDeath_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int IsSuddenDeath {
      get { return isSuddenDeath_; }
      set {
        isSuddenDeath_ = value;
      }
    }

    /// <summary>Field number for the "BattleAction" field.</summary>
    public const int BattleActionFieldNumber = 5;
    private static readonly pb::FieldCodec<global::Game.Protobuf.Types.BattleActionDataLog> _repeated_battleAction_codec
        = pb::FieldCodec.ForMessage(42, global::Game.Protobuf.Types.BattleActionDataLog.Parser);
    private readonly pbc::RepeatedField<global::Game.Protobuf.Types.BattleActionDataLog> battleAction_ = new pbc::RepeatedField<global::Game.Protobuf.Types.BattleActionDataLog>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Game.Protobuf.Types.BattleActionDataLog> BattleAction {
      get { return battleAction_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRoomBattleResult> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRoomBattleResult>.Equals(FightingRoomBattleResult other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ErrorCode != other.ErrorCode) return false;
      if (TicketPasscode != other.TicketPasscode) return false;
      if (Result != other.Result) return false;
      if (IsSuddenDeath != other.IsSuddenDeath) return false;
      if(!battleAction_.Equals(other.battleAction_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ErrorCode != 0) hash ^= ErrorCode.GetHashCode();
      if (TicketPasscode.Length != 0) hash ^= TicketPasscode.GetHashCode();
      if (Result != global::Game.Protobuf.Types.BattleResult.ResultError) hash ^= Result.GetHashCode();
      if (IsSuddenDeath != 0) hash ^= IsSuddenDeath.GetHashCode();
      hash ^= battleAction_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (ErrorCode != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ErrorCode);
      }
      if (TicketPasscode.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(TicketPasscode);
      }
      if (Result != global::Game.Protobuf.Types.BattleResult.ResultError) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Result);
      }
      if (IsSuddenDeath != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(IsSuddenDeath);
      }
      battleAction_.WriteTo(output, _repeated_battleAction_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ErrorCode != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ErrorCode);
      }
      if (TicketPasscode.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(TicketPasscode);
      }
      if (Result != global::Game.Protobuf.Types.BattleResult.ResultError) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Result);
      }
      if (IsSuddenDeath != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(IsSuddenDeath);
      }
      battleAction_.WriteTo(ref output, _repeated_battleAction_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (ErrorCode != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ErrorCode);
      }
      if (TicketPasscode.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TicketPasscode);
      }
      if (Result != global::Game.Protobuf.Types.BattleResult.ResultError) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Result);
      }
      if (IsSuddenDeath != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(IsSuddenDeath);
      }
      size += battleAction_.CalculateSize(_repeated_battleAction_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRoomBattleResult>.MergeFrom(FightingRoomBattleResult other) {
      if (other == null) {
        return;
      }
      if (other.ErrorCode != 0) {
        ErrorCode = other.ErrorCode;
      }
      if (other.TicketPasscode.Length != 0) {
        TicketPasscode = other.TicketPasscode;
      }
      if (other.Result != global::Game.Protobuf.Types.BattleResult.ResultError) {
        Result = other.Result;
      }
      if (other.IsSuddenDeath != 0) {
        IsSuddenDeath = other.IsSuddenDeath;
      }
      battleAction_.Add(other.battleAction_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            ErrorCode = input.ReadInt32();
            break;
          }
          case 18: {
            TicketPasscode = input.ReadString();
            break;
          }
          case 24: {
            Result = (global::Game.Protobuf.Types.BattleResult) input.ReadEnum();
            break;
          }
          case 32: {
            IsSuddenDeath = input.ReadInt32();
            break;
          }
          case 42: {
            battleAction_.AddEntriesFrom(input, _repeated_battleAction_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            ErrorCode = input.ReadInt32();
            break;
          }
          case 18: {
            TicketPasscode = input.ReadString();
            break;
          }
          case 24: {
            Result = (global::Game.Protobuf.Types.BattleResult) input.ReadEnum();
            break;
          }
          case 32: {
            IsSuddenDeath = input.ReadInt32();
            break;
          }
          case 42: {
            battleAction_.AddEntriesFrom(ref input, _repeated_battleAction_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class ReportFrameData : pb::IMessage<ReportFrameData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ReportFrameData> _parser = new pb::MessageParser<ReportFrameData>(() => new ReportFrameData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ReportFrameData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[19]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ReportFrameData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ReportFrameData(ReportFrameData other) : this() {
      frame_ = other.frame_;
      key_ = other.key_;
      location_ = other.location_;
      rotation_ = other.rotation_;
      hp_ = other.hp_;
      state_ = other.state_;
      move_ = other.move_;
      animation_ = other.animation_;
      keyEventArray_ = other.keyEventArray_;
      keyPoolArray_ = other.keyPoolArray_;
      event_ = other.event_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    ReportFrameData pb::IDeepCloneable<ReportFrameData>.Clone() {
      return new ReportFrameData(this);
    }

    /// <summary>Field number for the "Frame" field.</summary>
    public const int FrameFieldNumber = 1;
    private int frame_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Frame {
      get { return frame_; }
      set {
        frame_ = value;
      }
    }

    /// <summary>Field number for the "Key" field.</summary>
    public const int KeyFieldNumber = 2;
    private string key_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Key {
      get { return key_; }
      set {
        key_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Location" field.</summary>
    public const int LocationFieldNumber = 3;
    private string location_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Location {
      get { return location_; }
      set {
        location_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Rotation" field.</summary>
    public const int RotationFieldNumber = 4;
    private string rotation_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Rotation {
      get { return rotation_; }
      set {
        rotation_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Hp" field.</summary>
    public const int HpFieldNumber = 5;
    private int hp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Hp {
      get { return hp_; }
      set {
        hp_ = value;
      }
    }

    /// <summary>Field number for the "State" field.</summary>
    public const int StateFieldNumber = 6;
    private int state_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int State {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    /// <summary>Field number for the "Move" field.</summary>
    public const int MoveFieldNumber = 7;
    private int move_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Move {
      get { return move_; }
      set {
        move_ = value;
      }
    }

    /// <summary>Field number for the "Animation" field.</summary>
    public const int AnimationFieldNumber = 8;
    private int animation_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Animation {
      get { return animation_; }
      set {
        animation_ = value;
      }
    }

    /// <summary>Field number for the "KeyEventArray" field.</summary>
    public const int KeyEventArrayFieldNumber = 9;
    private string keyEventArray_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string KeyEventArray {
      get { return keyEventArray_; }
      set {
        keyEventArray_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "KeyPoolArray" field.</summary>
    public const int KeyPoolArrayFieldNumber = 10;
    private string keyPoolArray_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string KeyPoolArray {
      get { return keyPoolArray_; }
      set {
        keyPoolArray_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Event" field.</summary>
    public const int EventFieldNumber = 11;
    private string event_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Event {
      get { return event_; }
      set {
        event_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<ReportFrameData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<ReportFrameData>.Equals(ReportFrameData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Frame != other.Frame) return false;
      if (Key != other.Key) return false;
      if (Location != other.Location) return false;
      if (Rotation != other.Rotation) return false;
      if (Hp != other.Hp) return false;
      if (State != other.State) return false;
      if (Move != other.Move) return false;
      if (Animation != other.Animation) return false;
      if (KeyEventArray != other.KeyEventArray) return false;
      if (KeyPoolArray != other.KeyPoolArray) return false;
      if (Event != other.Event) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Frame != 0) hash ^= Frame.GetHashCode();
      if (Key.Length != 0) hash ^= Key.GetHashCode();
      if (Location.Length != 0) hash ^= Location.GetHashCode();
      if (Rotation.Length != 0) hash ^= Rotation.GetHashCode();
      if (Hp != 0) hash ^= Hp.GetHashCode();
      if (State != 0) hash ^= State.GetHashCode();
      if (Move != 0) hash ^= Move.GetHashCode();
      if (Animation != 0) hash ^= Animation.GetHashCode();
      if (KeyEventArray.Length != 0) hash ^= KeyEventArray.GetHashCode();
      if (KeyPoolArray.Length != 0) hash ^= KeyPoolArray.GetHashCode();
      if (Event.Length != 0) hash ^= Event.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Frame != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Frame);
      }
      if (Key.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Key);
      }
      if (Location.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Location);
      }
      if (Rotation.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Rotation);
      }
      if (Hp != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(Hp);
      }
      if (State != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(State);
      }
      if (Move != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(Move);
      }
      if (Animation != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(Animation);
      }
      if (KeyEventArray.Length != 0) {
        output.WriteRawTag(74);
        output.WriteString(KeyEventArray);
      }
      if (KeyPoolArray.Length != 0) {
        output.WriteRawTag(82);
        output.WriteString(KeyPoolArray);
      }
      if (Event.Length != 0) {
        output.WriteRawTag(90);
        output.WriteString(Event);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Frame != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Frame);
      }
      if (Key.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Key);
      }
      if (Location.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Location);
      }
      if (Rotation.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Rotation);
      }
      if (Hp != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(Hp);
      }
      if (State != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(State);
      }
      if (Move != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(Move);
      }
      if (Animation != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(Animation);
      }
      if (KeyEventArray.Length != 0) {
        output.WriteRawTag(74);
        output.WriteString(KeyEventArray);
      }
      if (KeyPoolArray.Length != 0) {
        output.WriteRawTag(82);
        output.WriteString(KeyPoolArray);
      }
      if (Event.Length != 0) {
        output.WriteRawTag(90);
        output.WriteString(Event);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (Frame != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Frame);
      }
      if (Key.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Key);
      }
      if (Location.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Location);
      }
      if (Rotation.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Rotation);
      }
      if (Hp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Hp);
      }
      if (State != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(State);
      }
      if (Move != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Move);
      }
      if (Animation != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Animation);
      }
      if (KeyEventArray.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(KeyEventArray);
      }
      if (KeyPoolArray.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(KeyPoolArray);
      }
      if (Event.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Event);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<ReportFrameData>.MergeFrom(ReportFrameData other) {
      if (other == null) {
        return;
      }
      if (other.Frame != 0) {
        Frame = other.Frame;
      }
      if (other.Key.Length != 0) {
        Key = other.Key;
      }
      if (other.Location.Length != 0) {
        Location = other.Location;
      }
      if (other.Rotation.Length != 0) {
        Rotation = other.Rotation;
      }
      if (other.Hp != 0) {
        Hp = other.Hp;
      }
      if (other.State != 0) {
        State = other.State;
      }
      if (other.Move != 0) {
        Move = other.Move;
      }
      if (other.Animation != 0) {
        Animation = other.Animation;
      }
      if (other.KeyEventArray.Length != 0) {
        KeyEventArray = other.KeyEventArray;
      }
      if (other.KeyPoolArray.Length != 0) {
        KeyPoolArray = other.KeyPoolArray;
      }
      if (other.Event.Length != 0) {
        Event = other.Event;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Frame = input.ReadInt32();
            break;
          }
          case 18: {
            Key = input.ReadString();
            break;
          }
          case 26: {
            Location = input.ReadString();
            break;
          }
          case 34: {
            Rotation = input.ReadString();
            break;
          }
          case 40: {
            Hp = input.ReadInt32();
            break;
          }
          case 48: {
            State = input.ReadInt32();
            break;
          }
          case 56: {
            Move = input.ReadInt32();
            break;
          }
          case 64: {
            Animation = input.ReadInt32();
            break;
          }
          case 74: {
            KeyEventArray = input.ReadString();
            break;
          }
          case 82: {
            KeyPoolArray = input.ReadString();
            break;
          }
          case 90: {
            Event = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Frame = input.ReadInt32();
            break;
          }
          case 18: {
            Key = input.ReadString();
            break;
          }
          case 26: {
            Location = input.ReadString();
            break;
          }
          case 34: {
            Rotation = input.ReadString();
            break;
          }
          case 40: {
            Hp = input.ReadInt32();
            break;
          }
          case 48: {
            State = input.ReadInt32();
            break;
          }
          case 56: {
            Move = input.ReadInt32();
            break;
          }
          case 64: {
            Animation = input.ReadInt32();
            break;
          }
          case 74: {
            KeyEventArray = input.ReadString();
            break;
          }
          case 82: {
            KeyPoolArray = input.ReadString();
            break;
          }
          case 90: {
            Event = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FightingPlayerReport : pb::IMessage<FightingPlayerReport>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingPlayerReport> _parser = new pb::MessageParser<FightingPlayerReport>(() => new FightingPlayerReport());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingPlayerReport> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[20]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingPlayerReport() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingPlayerReport(FightingPlayerReport other) : this() {
      roleId_ = other.roleId_;
      weaponId_ = other.weaponId_;
      subWeaponId_ = other.subWeaponId_;
      frameAction_ = other.frameAction_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingPlayerReport pb::IDeepCloneable<FightingPlayerReport>.Clone() {
      return new FightingPlayerReport(this);
    }

    /// <summary>Field number for the "RoleId" field.</summary>
    public const int RoleIdFieldNumber = 1;
    private int roleId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int RoleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    /// <summary>Field number for the "WeaponId" field.</summary>
    public const int WeaponIdFieldNumber = 2;
    private int weaponId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int WeaponId {
      get { return weaponId_; }
      set {
        weaponId_ = value;
      }
    }

    /// <summary>Field number for the "SubWeaponId" field.</summary>
    public const int SubWeaponIdFieldNumber = 3;
    private int subWeaponId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SubWeaponId {
      get { return subWeaponId_; }
      set {
        subWeaponId_ = value;
      }
    }

    /// <summary>Field number for the "FrameAction" field.</summary>
    public const int FrameActionFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Game.Protobuf.Types.ReportFrameData> _repeated_frameAction_codec
        = pb::FieldCodec.ForMessage(34, global::Game.Protobuf.Types.ReportFrameData.Parser);
    private readonly pbc::RepeatedField<global::Game.Protobuf.Types.ReportFrameData> frameAction_ = new pbc::RepeatedField<global::Game.Protobuf.Types.ReportFrameData>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Game.Protobuf.Types.ReportFrameData> FrameAction {
      get { return frameAction_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingPlayerReport> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingPlayerReport>.Equals(FightingPlayerReport other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RoleId != other.RoleId) return false;
      if (WeaponId != other.WeaponId) return false;
      if (SubWeaponId != other.SubWeaponId) return false;
      if(!frameAction_.Equals(other.frameAction_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (RoleId != 0) hash ^= RoleId.GetHashCode();
      if (WeaponId != 0) hash ^= WeaponId.GetHashCode();
      if (SubWeaponId != 0) hash ^= SubWeaponId.GetHashCode();
      hash ^= frameAction_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (RoleId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(RoleId);
      }
      if (WeaponId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(WeaponId);
      }
      if (SubWeaponId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(SubWeaponId);
      }
      frameAction_.WriteTo(output, _repeated_frameAction_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (RoleId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(RoleId);
      }
      if (WeaponId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(WeaponId);
      }
      if (SubWeaponId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(SubWeaponId);
      }
      frameAction_.WriteTo(ref output, _repeated_frameAction_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (RoleId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoleId);
      }
      if (WeaponId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(WeaponId);
      }
      if (SubWeaponId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SubWeaponId);
      }
      size += frameAction_.CalculateSize(_repeated_frameAction_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingPlayerReport>.MergeFrom(FightingPlayerReport other) {
      if (other == null) {
        return;
      }
      if (other.RoleId != 0) {
        RoleId = other.RoleId;
      }
      if (other.WeaponId != 0) {
        WeaponId = other.WeaponId;
      }
      if (other.SubWeaponId != 0) {
        SubWeaponId = other.SubWeaponId;
      }
      frameAction_.Add(other.frameAction_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            RoleId = input.ReadInt32();
            break;
          }
          case 16: {
            WeaponId = input.ReadInt32();
            break;
          }
          case 24: {
            SubWeaponId = input.ReadInt32();
            break;
          }
          case 34: {
            frameAction_.AddEntriesFrom(input, _repeated_frameAction_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            RoleId = input.ReadInt32();
            break;
          }
          case 16: {
            WeaponId = input.ReadInt32();
            break;
          }
          case 24: {
            SubWeaponId = input.ReadInt32();
            break;
          }
          case 34: {
            frameAction_.AddEntriesFrom(ref input, _repeated_frameAction_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class RoundPlayerReportData : pb::IMessage<RoundPlayerReportData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RoundPlayerReportData> _parser = new pb::MessageParser<RoundPlayerReportData>(() => new RoundPlayerReportData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<RoundPlayerReportData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[21]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RoundPlayerReportData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RoundPlayerReportData(RoundPlayerReportData other) : this() {
      round_ = other.round_;
      playerData_ = other.playerData_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    RoundPlayerReportData pb::IDeepCloneable<RoundPlayerReportData>.Clone() {
      return new RoundPlayerReportData(this);
    }

    /// <summary>Field number for the "Round" field.</summary>
    public const int RoundFieldNumber = 1;
    private int round_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Round {
      get { return round_; }
      set {
        round_ = value;
      }
    }

    /// <summary>Field number for the "PlayerData" field.</summary>
    public const int PlayerDataFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Game.Protobuf.Types.FightingPlayerReport> _repeated_playerData_codec
        = pb::FieldCodec.ForMessage(18, global::Game.Protobuf.Types.FightingPlayerReport.Parser);
    private readonly pbc::RepeatedField<global::Game.Protobuf.Types.FightingPlayerReport> playerData_ = new pbc::RepeatedField<global::Game.Protobuf.Types.FightingPlayerReport>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Game.Protobuf.Types.FightingPlayerReport> PlayerData {
      get { return playerData_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<RoundPlayerReportData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<RoundPlayerReportData>.Equals(RoundPlayerReportData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Round != other.Round) return false;
      if(!playerData_.Equals(other.playerData_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Round != 0) hash ^= Round.GetHashCode();
      hash ^= playerData_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Round != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Round);
      }
      playerData_.WriteTo(output, _repeated_playerData_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Round != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Round);
      }
      playerData_.WriteTo(ref output, _repeated_playerData_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (Round != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Round);
      }
      size += playerData_.CalculateSize(_repeated_playerData_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<RoundPlayerReportData>.MergeFrom(RoundPlayerReportData other) {
      if (other == null) {
        return;
      }
      if (other.Round != 0) {
        Round = other.Round;
      }
      playerData_.Add(other.playerData_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Round = input.ReadInt32();
            break;
          }
          case 18: {
            playerData_.AddEntriesFrom(input, _repeated_playerData_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Round = input.ReadInt32();
            break;
          }
          case 18: {
            playerData_.AddEntriesFrom(ref input, _repeated_playerData_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FightingRoomReport : pb::IMessage<FightingRoomReport>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FightingRoomReport> _parser = new pb::MessageParser<FightingRoomReport>(() => new FightingRoomReport());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FightingRoomReport> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[22]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomReport() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FightingRoomReport(FightingRoomReport other) : this() {
      battleReports_ = other.battleReports_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    FightingRoomReport pb::IDeepCloneable<FightingRoomReport>.Clone() {
      return new FightingRoomReport(this);
    }

    /// <summary>Field number for the "Battle_Reports" field.</summary>
    public const int BattleReportsFieldNumber = 1;
    private static readonly pb::FieldCodec<string> _repeated_battleReports_codec
        = pb::FieldCodec.ForString(10);
    private readonly pbc::RepeatedField<string> battleReports_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<string> BattleReports {
      get { return battleReports_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<FightingRoomReport> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<FightingRoomReport>.Equals(FightingRoomReport other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!battleReports_.Equals(other.battleReports_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= battleReports_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      battleReports_.WriteTo(output, _repeated_battleReports_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      battleReports_.WriteTo(ref output, _repeated_battleReports_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      size += battleReports_.CalculateSize(_repeated_battleReports_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<FightingRoomReport>.MergeFrom(FightingRoomReport other) {
      if (other == null) {
        return;
      }
      battleReports_.Add(other.battleReports_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            battleReports_.AddEntriesFrom(input, _repeated_battleReports_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            battleReports_.AddEntriesFrom(ref input, _repeated_battleReports_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class ProtoVector : pb::IMessage<ProtoVector>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ProtoVector> _parser = new pb::MessageParser<ProtoVector>(() => new ProtoVector());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ProtoVector> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[23]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ProtoVector() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ProtoVector(ProtoVector other) : this() {
      x_ = other.x_;
      y_ = other.y_;
      z_ = other.z_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    ProtoVector pb::IDeepCloneable<ProtoVector>.Clone() {
      return new ProtoVector(this);
    }

    /// <summary>Field number for the "X" field.</summary>
    public const int XFieldNumber = 1;
    private int x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "Y" field.</summary>
    public const int YFieldNumber = 2;
    private int y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "Z" field.</summary>
    public const int ZFieldNumber = 3;
    private int z_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Z {
      get { return z_; }
      set {
        z_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<ProtoVector> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<ProtoVector>.Equals(ProtoVector other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (X != other.X) return false;
      if (Y != other.Y) return false;
      if (Z != other.Z) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0) hash ^= X.GetHashCode();
      if (Y != 0) hash ^= Y.GetHashCode();
      if (Z != 0) hash ^= Z.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (X != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(X);
      }
      if (Y != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Y);
      }
      if (Z != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Z);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (X != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(X);
      }
      if (Y != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Y);
      }
      if (Z != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Z);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (X != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(X);
      }
      if (Y != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Y);
      }
      if (Z != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Z);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<ProtoVector>.MergeFrom(ProtoVector other) {
      if (other == null) {
        return;
      }
      if (other.X != 0) {
        X = other.X;
      }
      if (other.Y != 0) {
        Y = other.Y;
      }
      if (other.Z != 0) {
        Z = other.Z;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            X = input.ReadInt32();
            break;
          }
          case 16: {
            Y = input.ReadInt32();
            break;
          }
          case 24: {
            Z = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            X = input.ReadInt32();
            break;
          }
          case 16: {
            Y = input.ReadInt32();
            break;
          }
          case 24: {
            Z = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class ProtoRotate : pb::IMessage<ProtoRotate>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ProtoRotate> _parser = new pb::MessageParser<ProtoRotate>(() => new ProtoRotate());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ProtoRotate> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[24]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ProtoRotate() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ProtoRotate(ProtoRotate other) : this() {
      pitch_ = other.pitch_;
      yaw_ = other.yaw_;
      roll_ = other.roll_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    ProtoRotate pb::IDeepCloneable<ProtoRotate>.Clone() {
      return new ProtoRotate(this);
    }

    /// <summary>Field number for the "Pitch" field.</summary>
    public const int PitchFieldNumber = 1;
    private int pitch_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Pitch {
      get { return pitch_; }
      set {
        pitch_ = value;
      }
    }

    /// <summary>Field number for the "Yaw" field.</summary>
    public const int YawFieldNumber = 2;
    private int yaw_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Yaw {
      get { return yaw_; }
      set {
        yaw_ = value;
      }
    }

    /// <summary>Field number for the "Roll" field.</summary>
    public const int RollFieldNumber = 3;
    private int roll_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Roll {
      get { return roll_; }
      set {
        roll_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<ProtoRotate> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<ProtoRotate>.Equals(ProtoRotate other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Pitch != other.Pitch) return false;
      if (Yaw != other.Yaw) return false;
      if (Roll != other.Roll) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Pitch != 0) hash ^= Pitch.GetHashCode();
      if (Yaw != 0) hash ^= Yaw.GetHashCode();
      if (Roll != 0) hash ^= Roll.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Pitch != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Pitch);
      }
      if (Yaw != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Yaw);
      }
      if (Roll != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Roll);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Pitch != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Pitch);
      }
      if (Yaw != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Yaw);
      }
      if (Roll != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Roll);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (Pitch != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Pitch);
      }
      if (Yaw != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Yaw);
      }
      if (Roll != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Roll);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<ProtoRotate>.MergeFrom(ProtoRotate other) {
      if (other == null) {
        return;
      }
      if (other.Pitch != 0) {
        Pitch = other.Pitch;
      }
      if (other.Yaw != 0) {
        Yaw = other.Yaw;
      }
      if (other.Roll != 0) {
        Roll = other.Roll;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Pitch = input.ReadInt32();
            break;
          }
          case 16: {
            Yaw = input.ReadInt32();
            break;
          }
          case 24: {
            Roll = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Pitch = input.ReadInt32();
            break;
          }
          case 16: {
            Yaw = input.ReadInt32();
            break;
          }
          case 24: {
            Roll = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class BattleReportStateData : pb::IMessage<BattleReportStateData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<BattleReportStateData> _parser = new pb::MessageParser<BattleReportStateData>(() => new BattleReportStateData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<BattleReportStateData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[25]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattleReportStateData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattleReportStateData(BattleReportStateData other) : this() {
      vactor_ = other.vactor_ != null ? other.vactor_.Clone() : null;
      rotate_ = other.rotate_ != null ? other.rotate_.Clone() : null;
      hp_ = other.hp_;
      fighterStateCtrlID_ = other.fighterStateCtrlID_;
      keyDownFlag_ = other.keyDownFlag_;
      keyDownFlagLast_ = other.keyDownFlagLast_;
      aminID_ = other.aminID_;
      moveID_ = other.moveID_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    BattleReportStateData pb::IDeepCloneable<BattleReportStateData>.Clone() {
      return new BattleReportStateData(this);
    }

    /// <summary>Field number for the "Vactor" field.</summary>
    public const int VactorFieldNumber = 1;
    private global::Game.Protobuf.Types.ProtoVector vactor_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.ProtoVector Vactor {
      get { return vactor_; }
      set {
        vactor_ = value;
      }
    }

    /// <summary>Field number for the "Rotate" field.</summary>
    public const int RotateFieldNumber = 2;
    private global::Game.Protobuf.Types.ProtoRotate rotate_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Game.Protobuf.Types.ProtoRotate Rotate {
      get { return rotate_; }
      set {
        rotate_ = value;
      }
    }

    /// <summary>Field number for the "Hp" field.</summary>
    public const int HpFieldNumber = 3;
    private int hp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Hp {
      get { return hp_; }
      set {
        hp_ = value;
      }
    }

    /// <summary>Field number for the "FighterStateCtrlID" field.</summary>
    public const int FighterStateCtrlIDFieldNumber = 4;
    private int fighterStateCtrlID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int FighterStateCtrlID {
      get { return fighterStateCtrlID_; }
      set {
        fighterStateCtrlID_ = value;
      }
    }

    /// <summary>Field number for the "KeyDownFlag" field.</summary>
    public const int KeyDownFlagFieldNumber = 5;
    private uint keyDownFlag_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint KeyDownFlag {
      get { return keyDownFlag_; }
      set {
        keyDownFlag_ = value;
      }
    }

    /// <summary>Field number for the "KeyDownFlagLast" field.</summary>
    public const int KeyDownFlagLastFieldNumber = 6;
    private uint keyDownFlagLast_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint KeyDownFlagLast {
      get { return keyDownFlagLast_; }
      set {
        keyDownFlagLast_ = value;
      }
    }

    /// <summary>Field number for the "AminID" field.</summary>
    public const int AminIDFieldNumber = 7;
    private int aminID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int AminID {
      get { return aminID_; }
      set {
        aminID_ = value;
      }
    }

    /// <summary>Field number for the "MoveID" field.</summary>
    public const int MoveIDFieldNumber = 8;
    private int moveID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int MoveID {
      get { return moveID_; }
      set {
        moveID_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<BattleReportStateData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<BattleReportStateData>.Equals(BattleReportStateData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Vactor, other.Vactor)) return false;
      if (!object.Equals(Rotate, other.Rotate)) return false;
      if (Hp != other.Hp) return false;
      if (FighterStateCtrlID != other.FighterStateCtrlID) return false;
      if (KeyDownFlag != other.KeyDownFlag) return false;
      if (KeyDownFlagLast != other.KeyDownFlagLast) return false;
      if (AminID != other.AminID) return false;
      if (MoveID != other.MoveID) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (vactor_ != null) hash ^= Vactor.GetHashCode();
      if (rotate_ != null) hash ^= Rotate.GetHashCode();
      if (Hp != 0) hash ^= Hp.GetHashCode();
      if (FighterStateCtrlID != 0) hash ^= FighterStateCtrlID.GetHashCode();
      if (KeyDownFlag != 0) hash ^= KeyDownFlag.GetHashCode();
      if (KeyDownFlagLast != 0) hash ^= KeyDownFlagLast.GetHashCode();
      if (AminID != 0) hash ^= AminID.GetHashCode();
      if (MoveID != 0) hash ^= MoveID.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (vactor_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Vactor);
      }
      if (rotate_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Rotate);
      }
      if (Hp != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Hp);
      }
      if (FighterStateCtrlID != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(FighterStateCtrlID);
      }
      if (KeyDownFlag != 0) {
        output.WriteRawTag(40);
        output.WriteUInt32(KeyDownFlag);
      }
      if (KeyDownFlagLast != 0) {
        output.WriteRawTag(48);
        output.WriteUInt32(KeyDownFlagLast);
      }
      if (AminID != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(AminID);
      }
      if (MoveID != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(MoveID);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (vactor_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Vactor);
      }
      if (rotate_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Rotate);
      }
      if (Hp != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Hp);
      }
      if (FighterStateCtrlID != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(FighterStateCtrlID);
      }
      if (KeyDownFlag != 0) {
        output.WriteRawTag(40);
        output.WriteUInt32(KeyDownFlag);
      }
      if (KeyDownFlagLast != 0) {
        output.WriteRawTag(48);
        output.WriteUInt32(KeyDownFlagLast);
      }
      if (AminID != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(AminID);
      }
      if (MoveID != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(MoveID);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (vactor_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Vactor);
      }
      if (rotate_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Rotate);
      }
      if (Hp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Hp);
      }
      if (FighterStateCtrlID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(FighterStateCtrlID);
      }
      if (KeyDownFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(KeyDownFlag);
      }
      if (KeyDownFlagLast != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(KeyDownFlagLast);
      }
      if (AminID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(AminID);
      }
      if (MoveID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MoveID);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<BattleReportStateData>.MergeFrom(BattleReportStateData other) {
      if (other == null) {
        return;
      }
      if (other.vactor_ != null) {
        if (vactor_ == null) {
          Vactor = new global::Game.Protobuf.Types.ProtoVector();
        }
        Vactor.MergeFrom(other.Vactor);
      }
      if (other.rotate_ != null) {
        if (rotate_ == null) {
          Rotate = new global::Game.Protobuf.Types.ProtoRotate();
        }
        Rotate.MergeFrom(other.Rotate);
      }
      if (other.Hp != 0) {
        Hp = other.Hp;
      }
      if (other.FighterStateCtrlID != 0) {
        FighterStateCtrlID = other.FighterStateCtrlID;
      }
      if (other.KeyDownFlag != 0) {
        KeyDownFlag = other.KeyDownFlag;
      }
      if (other.KeyDownFlagLast != 0) {
        KeyDownFlagLast = other.KeyDownFlagLast;
      }
      if (other.AminID != 0) {
        AminID = other.AminID;
      }
      if (other.MoveID != 0) {
        MoveID = other.MoveID;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (vactor_ == null) {
              Vactor = new global::Game.Protobuf.Types.ProtoVector();
            }
            input.ReadMessage(Vactor);
            break;
          }
          case 18: {
            if (rotate_ == null) {
              Rotate = new global::Game.Protobuf.Types.ProtoRotate();
            }
            input.ReadMessage(Rotate);
            break;
          }
          case 24: {
            Hp = input.ReadInt32();
            break;
          }
          case 32: {
            FighterStateCtrlID = input.ReadInt32();
            break;
          }
          case 40: {
            KeyDownFlag = input.ReadUInt32();
            break;
          }
          case 48: {
            KeyDownFlagLast = input.ReadUInt32();
            break;
          }
          case 56: {
            AminID = input.ReadInt32();
            break;
          }
          case 64: {
            MoveID = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (vactor_ == null) {
              Vactor = new global::Game.Protobuf.Types.ProtoVector();
            }
            input.ReadMessage(Vactor);
            break;
          }
          case 18: {
            if (rotate_ == null) {
              Rotate = new global::Game.Protobuf.Types.ProtoRotate();
            }
            input.ReadMessage(Rotate);
            break;
          }
          case 24: {
            Hp = input.ReadInt32();
            break;
          }
          case 32: {
            FighterStateCtrlID = input.ReadInt32();
            break;
          }
          case 40: {
            KeyDownFlag = input.ReadUInt32();
            break;
          }
          case 48: {
            KeyDownFlagLast = input.ReadUInt32();
            break;
          }
          case 56: {
            AminID = input.ReadInt32();
            break;
          }
          case 64: {
            MoveID = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class BattleReportPlayerData : pb::IMessage<BattleReportPlayerData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<BattleReportPlayerData> _parser = new pb::MessageParser<BattleReportPlayerData>(() => new BattleReportPlayerData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<BattleReportPlayerData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Game.Protobuf.Types.FightingRoomProtoReflection.Descriptor.MessageTypes[26]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattleReportPlayerData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattleReportPlayerData(BattleReportPlayerData other) : this() {
      selfPlayerId_ = other.selfPlayerId_;
      round_ = other.round_;
      frame_ = other.frame_;
      playerStateData_ = other.playerStateData_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    BattleReportPlayerData pb::IDeepCloneable<BattleReportPlayerData>.Clone() {
      return new BattleReportPlayerData(this);
    }

    /// <summary>Field number for the "SelfPlayerId" field.</summary>
    public const int SelfPlayerIdFieldNumber = 1;
    private int selfPlayerId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SelfPlayerId {
      get { return selfPlayerId_; }
      set {
        selfPlayerId_ = value;
      }
    }

    /// <summary>Field number for the "Round" field.</summary>
    public const int RoundFieldNumber = 2;
    private int round_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Round {
      get { return round_; }
      set {
        round_ = value;
      }
    }

    /// <summary>Field number for the "Frame" field.</summary>
    public const int FrameFieldNumber = 3;
    private int frame_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Frame {
      get { return frame_; }
      set {
        frame_ = value;
      }
    }

    /// <summary>Field number for the "PlayerStateData" field.</summary>
    public const int PlayerStateDataFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Game.Protobuf.Types.BattleReportStateData> _repeated_playerStateData_codec
        = pb::FieldCodec.ForMessage(34, global::Game.Protobuf.Types.BattleReportStateData.Parser);
    private readonly pbc::RepeatedField<global::Game.Protobuf.Types.BattleReportStateData> playerStateData_ = new pbc::RepeatedField<global::Game.Protobuf.Types.BattleReportStateData>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Game.Protobuf.Types.BattleReportStateData> PlayerStateData {
      get { return playerStateData_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return (other is global::System.IEquatable<BattleReportPlayerData> another) ? another.Equals(this) : false;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    bool global::System.IEquatable<BattleReportPlayerData>.Equals(BattleReportPlayerData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SelfPlayerId != other.SelfPlayerId) return false;
      if (Round != other.Round) return false;
      if (Frame != other.Frame) return false;
      if(!playerStateData_.Equals(other.playerStateData_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (SelfPlayerId != 0) hash ^= SelfPlayerId.GetHashCode();
      if (Round != 0) hash ^= Round.GetHashCode();
      if (Frame != 0) hash ^= Frame.GetHashCode();
      hash ^= playerStateData_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (SelfPlayerId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(SelfPlayerId);
      }
      if (Round != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Round);
      }
      if (Frame != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Frame);
      }
      playerStateData_.WriteTo(output, _repeated_playerStateData_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (SelfPlayerId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(SelfPlayerId);
      }
      if (Round != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Round);
      }
      if (Frame != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Frame);
      }
      playerStateData_.WriteTo(ref output, _repeated_playerStateData_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    int pb::IMessage.CalculateSize() {
      int size = 0;
      if (SelfPlayerId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SelfPlayerId);
      }
      if (Round != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Round);
      }
      if (Frame != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Frame);
      }
      size += playerStateData_.CalculateSize(_repeated_playerStateData_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage<BattleReportPlayerData>.MergeFrom(BattleReportPlayerData other) {
      if (other == null) {
        return;
      }
      if (other.SelfPlayerId != 0) {
        SelfPlayerId = other.SelfPlayerId;
      }
      if (other.Round != 0) {
        Round = other.Round;
      }
      if (other.Frame != 0) {
        Frame = other.Frame;
      }
      playerStateData_.Add(other.playerStateData_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IMessage.MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            SelfPlayerId = input.ReadInt32();
            break;
          }
          case 16: {
            Round = input.ReadInt32();
            break;
          }
          case 24: {
            Frame = input.ReadInt32();
            break;
          }
          case 34: {
            playerStateData_.AddEntriesFrom(input, _repeated_playerStateData_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            SelfPlayerId = input.ReadInt32();
            break;
          }
          case 16: {
            Round = input.ReadInt32();
            break;
          }
          case 24: {
            Frame = input.ReadInt32();
            break;
          }
          case 34: {
            playerStateData_.AddEntriesFrom(ref input, _repeated_playerStateData_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
