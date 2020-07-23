<?php

function listener($roomId, $clientTick, $playerId)
{
    $endTime = time() + (int) ini_get('max_execution_time') - 1;
    $mem = new MemoryReader($roomId);
    $room = new Room();
    
    while (time() < $endTime) {                
        sleep(0.2);        
        $room->read($mem);

        if (is_null($room->id)) continue;
        if ($room->expiration < time()) continue;
        if ($room->tick <= $clientTick) continue;
        if (!$room->balls) continue;
        Response::Merge($room);
        return;        
    }
}

?>