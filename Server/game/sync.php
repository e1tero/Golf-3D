<?php

function initializeRoom($room, $player)
{
	$time = time();
	if (is_null($room->balls) || $room->expiration < $time)
	{                        
        $room->balls = array();
        $room->tick = 0;        
    }

    $room->expiration = $time + 60;
    $room->tick++; 
   
    $balls = array();
    $playerBall = null;
    $index = 1;

    foreach ($room->balls as $ball) {
		if ($ball->player == $player)
        {
            $playerBall = $ball;
            $ball->tick = $room->tick;
            $ball->expiration = $time + 30;
        }

        if ($ball->expiration > $room->tick)
        {
        	$balls[] = $ball;
        	if ($index <= $ball->player)
        		$index = $ball->player + 1;
        }
    }
    $room->balls = $balls;

    if (is_null($playerBall))
    {
        $playerBall = new Ball();
        $playerBall->player = $index;
        $playerBall->expiration = $time + 30;
        $playerBall->events = array();
        $room->balls[] = $playerBall;
    }

    if (count($playerBall->events) > 3)    
		array_shift($playerBall->events);

    $playerBall->tick = $room->tick;
    return $playerBall;
}

function stayBall($ball, $position)
{
	$event = new Event();
	$event->tick = $ball->tick;
	$event->position = $position;	
	$ball->events[] = $event;	
}

function impulseBall($ball, $position, $direction, $power)
{
	$event = new Event();
	$event->tick = $ball->tick;	
	$event->position = $position;
	$event->direction = $direction;	
	$event->power = $power;
	$ball->events[] = $event;
}

?>