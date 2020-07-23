<?php

require_once("references.php");
require_once("common/shm_memory.php");
require_once("game/room.php");
require_once("game/listener.php");
require_once("game/sync.php");

try
{  
    $m = $_REQUEST["m"];
    $uid = $_REQUEST["uid"];
    $device = $_REQUEST["device"];
    $data = json_decode($_REQUEST["body"]);
    $roomId = $data->id;    

    switch ($m) {
        case 'ROOM_LISTEN':
            listener($roomId, $data->tick);
            break;
        case 'ROOM_CLEAR':
            $mem = new MemoryWriter($roomId);
            $room = new Room();
            $room->write($mem);
            break;
        case 'ROOM_JOIN':
            $mem = new MemoryWriter($roomId);
            $room = new Room();
            $room->read($mem);
            $room->id = $roomId;
            $playerBall = initializeRoom($room, 0);
            Response::Merge($room);
            Response::Add("player", $playerBall->player);
            break;
        case 'ROOM_LEAVE':            
            break;
        case 'ROOM_PUSH':          
            $player = $data->player;
            $mem = new MemoryWriter($roomId);
            $room = new Room();
            $room->read($mem);
            $room->id = $roomId;
            $playerBall = initializeRoom($room, $player);            
            impulseBall($playerBall, $data->position, $data->direction, $data->power);
            $room->write($mem);
            Response::Merge($playerBall);
            break;
        case 'ROOM_STAY':           
            $player = $data->player;             
            $mem = new MemoryWriter($roomId);
            $room = new Room();
            $room->read($mem);
            $room->id = $roomId;
            $playerBall = initializeRoom($room, $player);            
            stayBall($playerBall, $data);            
            $room->write($mem);
            Response::Merge($playerBall);
            break;
    }        

    echo Response::ToJSON(Config::$hash_key);  
}
// catch (ThrowDataBaseException $e)
// {
//     echo '{"error": "Internal Server Error", "code": "'.$e->getLine().'"}';
// }
catch (ServiceError $e)
{    
    echo Response::ToJSON(Config::$hash_key);
}
catch (Exception $e)
{
    echo '{"error": "'.$e->getMessage().'"}';
}

?>