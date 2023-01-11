<?php
include('Database.php');

abstract class admin
{
    public $admin_id;
    public $admin_name;
    public $admin_passwd;
    public $role;

    public function __construct($admin_id, $admin_name, $admin_passwd, $role)
    {
        $this->admin_id = $admin_id;
        $this->admin_name = $admin_name;
        $this->admin_passwd = $admin_passwd;
        $this->role = $role;

    }

    public function updatepswd ($newpswd){
        $db = new Database();

        if($db->updateadminpswd($this->admin_id,$this->admin_passwd,$newpswd)){
            $this->admin_passwd = $newpswd;
            unset($db);
            return true;
        }

        unset($db);
        return false;
    }

    public function verifypswd($pswd){
        if($this->admin_passwd == $pswd){
            return true;
        }
        return false;
    }

    public function getvoicedata($complaintno){
        
        $db = new Database();
        
        $resultobj = $db->getvoicedata($complaintno);

        $fd = $resultobj['filedata'];
        $fname = $resultobj['filename'];
        //echo $fname;
        $filedata = base64_decode($fd);

        $file = fopen("./localres/Voice/$fname.wav", "w+");
        fwrite($file, $filedata);
        fclose($file);

        unset($db);
        return "./localres/Voice/$fname.wav";
    }

    public function getcomplaintdata($status)
    {
        if ($this->role > 5) {
            return null;
        }
        if (is_a($this, 'SuperAdmin')) {
            $this->childintegrationcheck();
        }

        
        $db = new Database();
        $result = null;
        if ($status == 0) {
            $result = $db->getopencomplaintdata();
        } else if ($status == 1) {
            $result = $db->getclosecomplaintdata();
        }
        unset($db);
        return $result;
    }
}

class HOD extends admin
{
    public function __construct($admin_id, $admin_name, $admin_passwd, $role)
    {
        parent::__construct($admin_id, $admin_name, $admin_passwd, $role);
    }

    public function getcomplaintdata($status)
    {
        echo "HOD";
        
        $db = new Database();
        $result = null;
        if ($status == 0) {
            $result = $db->getopenspecificDepartmentdata($this->role);
        } else if ($status == 1) {
            $result = $db->getclosespecificDepartmentdata($this->role);
        }
        unset($db);
        return $result;
    }
}

class BaseAdmin extends admin
{
    public function __construct($admin_id, $admin_name, $admin_passwd, $role)
    {
        parent::__construct($admin_id, $admin_name, $admin_passwd, $role);
    }
}

class SuperAdmin extends admin
{
    public function __construct($admin_id, $admin_name, $admin_passwd, $role)
    {
        parent::__construct($admin_id, $admin_name, $admin_passwd, $role);
    }

    public function childintegrationcheck()
    {
        echo "Child Integration Check is done.\n";
        # code...
    }

    public function adddepartment($departmentname)
    {
        
        $db = new Database();
        $db->adddepartment($departmentname);
        unset($db);
    }

    public function addadmin($admin_id,$admin_passwd,$admin_name,$role){
        //
        $db = new Database();

        try{
            $db->addadmin($admin_id,$admin_passwd,$admin_name,$role);
        }
        catch(Exception $e){
            //echo $e->getMessage();
            return false;
        }
        
        unset($db);
        return true;
    }

    public function deldepartment($departmentid){
        
        $db = new Database();

        try{
            $db->deletedepartment($departmentid);
        }
        catch(Exception $e){
            //echo $e->getMessage();
            return false;
        }
        
        unset($db);
        return true;
    }
}

class AdminFactory
{
    public static function createAdmin($admin_id, $admin_name, $admin_passwd, $role)
    {
        echo $role;
        if ($role == 0) {
            return new SuperAdmin($admin_id, $admin_name, $admin_passwd, $role);
        } else if ($role == 1) {
            return new BaseAdmin($admin_id, $admin_name, $admin_passwd, $role);
        } else if ($role >= 5) {
            return new HOD($admin_id, $admin_name, $admin_passwd, $role);
        }
    }
}


?>