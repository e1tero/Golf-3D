<?php

Response::Create();
$_Result = Response::$Data;

class Response extends ArrayObject{
    public static $Data;
    public static $Error;    
    public static $Code;

    function __construct()
    {
        parent::__construct(array(), ArrayObject::ARRAY_AS_PROPS);
    }

    public static function Create()
    {
        Response::$Data = new Response();
        Response::$Code = 0;
        Response::$Error = "";
    }

    public static function ToJSON($key)
    {
        $res = new ArrayObject();
        $res["error"] = Response::$Error;
        $res["code"] = Response::$Code;
        $res["body"] = Response::$Data;
        $res["date"] = date("Y.m.d H:i:s", strtotime("now"));
        $res["hash"] = md5($res["date"].$key);
        return json_encode($res, JSON_UNESCAPED_UNICODE);
    }

    public static function Merge($array)
    {
        foreach ($array as $key=>$value){
            Response::$Data[$key] = $value;
        }
    }

    public static function LogTrace($msg)
    {
        if (!isset(Response::$Data->logs))
            Response::$Data->logs = array();

        Response::$Data->logs[] = $msg;
    }

    public static function Add($key, $value)
    {
        Response::$Data[$key] = $value;
    }

    public static function Get($key)
    {
        return Response::$Data[$key];
    }

    public static function Error($msg, $code)
    {
        Response::$Error = $msg;
        Response::$Code = $code;
        throw new ServiceError();
    }
}

class ServiceError extends Exception {

}

class Chooser
{
    private $total;
    private $items;


    public function __construct($items = [])
    {
        $this->items = $items;
        foreach ($items as $item) {
            $this->total += $item["chance"];
        }
    }

    public function Add($item, $chance)
    {
        $this->total += $chance;
        $this->items[] = [ "chance" => $item, "value" => $item ];
    }

    public function Choice()
    {
        $num = random_int(0, $this->total);
        foreach ($this->items as $item) {
            if ($num < $item["chance"]) return $item;
            else $num -= $item["chance"];
        }
        return null;
    }

    public function Clear()
    {
        $this->otal = 0;
        $this->items = [];
    }
}

?>