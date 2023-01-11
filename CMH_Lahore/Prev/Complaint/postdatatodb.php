<?php
$Comp_Data = false;
try {
    include('../db.php');
    // Check connection
    if ($conn->connect_error) {
        die("Database Connection failed: " . $conn->connect_error);
    } else {
        error_reporting(E_ERROR | E_PARSE);
        session_start();

        //write query to insert data into database table complaints 
        $cname = $_SESSION['cname'];
        $phone = $_SESSION['phone'];
        $DateofComplaint = $_SESSION['DateofComplaint'];
        $cnic = $_SESSION['cnic'];
        $dname = $_SESSION['dname'];
        $departname = $_SESSION['departname'];
        $Roomno = $_SESSION['Roomno'];
        $DateofIssue = $_SESSION['DateofIssue'];
        $complaint = '';
        
        if (isset($_POST['complaint'])) {
            $complaint = $_POST['complaint'];
            if ($complaint == null || strlen($complaint) <= 30) {
                $complaint = 'Voice Data';
                $Comp_Data = true;
            }
        } else {
            $complaint = 'Voice Data';
            $Comp_Data = true;
        }

        $_SESSION['check_post'] = false;
        $_SESSION['complaintno'] = 0;

        if (strlen($cname) == 0 || strlen($phone) == 0 || strlen($dname) == 0 || strlen($departname) == 0 || strlen($DateofIssue) == 0) {
            throw new Exception("Please fill all the fields");
        } else {
            try {
                $sql = "INSERT INTO `complaints`(`CName`, `Phone`, `DateofComplaint`, `Ccnic`, `Docname`, `Department_id`, `RoomNo`, `DateofIssue`, `Complaint`, `Status`, `Feedback`) VALUES ('$cname','$phone',NULL,'$cnic','$dname','$departname','$Roomno','$DateofIssue','$complaint',0,'')";
                mysqli_query($conn, $sql);

                $_SESSION['complaintno'] = $conn->insert_id;

                $conn->close();

                $_SESSION['check_post'] = true;

            } catch (Exception $e) {
                $errorofServer = 'Caught exception: ' + $e->getMessage() + "\nSolution: Contact the admin";
            }
        }
    }
} catch (Exception $e) {
    $msg = $e->getMessage();
}
?>