<?php

class Ball
{
	public $expiration;
	public $tick;
	public $player;	
	public $events;
}

class Event
{
	public $tick;
	public $position;
	public $direction;
	public $power;
}

class Vector3
{
	public $x;
	public $y;
	public $z;

	public function __construct($x = 0, $y = 0, $z = 0)
	{
		$this->x = $x;
		$this->y = $y;
		$this->z = $z;
	}
}

class Room
{	
	public $expiration;
	public $tick;
	public $balls;

	public function read($mem)
	{
		$this->id = $mem->get(0);
		$this->expiration = $mem->get(1);
		$this->tick = $mem->get(2);
		$this->balls = unserialize($mem->get(3));		
	}

	public function write($mem)
	{
		$mem->set(0, $this->id);		
		$mem->set(1, $this->expiration);
		$mem->set(2, $this->tick);
		$mem->set(3, serialize($this->balls));		
	}
}

?>