<?php
include('./postdatatodb.php');
session_start();

//echo "Complaint Number : " . $_SESSION['complaintno'];

//print_r($_FILES);
$input = $_FILES['audio_data']['tmp_name'];
//echo $input;
$output = $_FILES['audio_data']['name'] . ".wav";
//echo 'Output File : '.$output;
$cmpobj;

if (isset($_SESSION['check_post']) && $_SESSION['check_post'] == true && isset($_SESSION['complaintno'])) {
    $complaintno = $_SESSION['complaintno'];
    $cmpobj = "./Voice/" . $complaintno . ".wav";
    //echo 'Output File : ' . $output;

    if (move_uploaded_file($input, $cmpobj)) {
        
        $file_data = file_get_contents($cmpobj);
        $file_data = base64_encode($file_data);

        $filename = $complaintno;
        try {
            include('../db.php');

            $sql = "INSERT INTO `voicedata`(`filename`, `filedata`) VALUES ('$filename','$file_data')";
            mysqli_query($conn, $sql);
            $pkey = $conn->insert_id;
            //Voice Data Inserted Here  

            $conn->close();

            unlink($cmpobj);
        } catch (Exception $e) {
            $errorofServer = 'Caught exception: ' . $e->getMessage() . "\nSolution: Contact the admin";
            echo $errorofServer;
        }
    } else {
        echo "Error in uploading file";
    }
}
?>