<?php

if(function_exists("shm_attach")===FALSE){
    die("\nYour PHP configuration needs adjustment. Enable the System V shared memory support compile PHP with the option --enable-sysvshm.");
}

class MemoryReader
{   
    protected $id;
    protected $shm;
    protected $sem;

    public function __construct($id)
    {                            
        $this->id = $id;
        $this->shm = shm_attach($this->id);
        $this->sem = sem_get($id);
    }

    public function __wakeup() 
    {        
        $this->shm = shm_attach($this->id);
    }

    public function __destruct()
    {
        shm_detach($this->shm);
    }

    public function get($id)
    {
        sem_acquire($this->sem);
        $res = null;
        if($this->has($id))
            $res = shm_get_var($this->shm, $id);
        sem_release($this->sem);
        return $res;
    }

    public function has($id)
    {
        return shm_has_var($this->shm, $id);
    }
}

class MemoryWriter extends MemoryReader
{       
    public function __construct($id)
    {                            
        parent::__construct($id);        
        sem_acquire($this->sem);
    }

    public function __destruct()
    {
        parent::__destruct();
        sem_release($this->sem);
    }

    public function remove($id){
        if($this->has($id)){
            return shm_remove_var($this->shm, $id);
        }else{
            return false;
        }
    }

    public function get($id)
    {
        if($this->has($id))
            return shm_get_var($this->shm, $id);
        return null;     
    }

    public function set($id, $var) {
        return shm_put_var($this->shm, $id, $var); 
    }
}

?>