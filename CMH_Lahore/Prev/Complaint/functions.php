<?php
function getDepartments()
{
    include('../db.php');
    $check = false;

    //error_reporting(E_ERROR | E_PARSE);

    try {
        $sql = "SELECT * FROM `departments`";
        $result = mysqli_query($conn, $sql);
    } catch (Exception $e) {
        echo 'Caught exception: ', $e->getMessage(), "\n";
    }
    //close db here
    mysqli_close($conn);
    return $result;
}
?>