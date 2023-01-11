<!DOCTYPE html>
<html>

<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <script src="..\res\Tailwind.js"></script>
    <link rel="stylesheet" href="../res/Custom.css">
</head>

<body>
  <?php

  include('../db.php');
  $check = false;

  //error_reporting(E_ERROR | E_PARSE);
  
  $complaintNumber = $_POST['complaintNumber'];
  try {
    $sql = "SELECT * FROM `complaints` WHERE `ComplaintID` = $complaintNumber";
    $result = mysqli_query($conn, $sql);

    if ($result->num_rows > 0) {
      $check = true;
      // output data of each row
      $row = $result->fetch_assoc();
    }

    $conn->close();

  } catch (Exception $e) {
    //$errorofServer = 'Caught exception: ' + $e->getMessage() + "\nSolution: Contact the admin";
  }
  ?>
  <script src="https://kit.fontawesome.com/8ed36584a1.js" crossorigin="anonymous"></script>
  <div class="flex flex-col w-screen h-screen justify-center items-center">
    <div class="flex flex-row justify-evenly items-center w-screen h-screen">
      <div class="w-2/12">
        <img src="..\res\img\logo.png" />
      </div>
      <div>
        <!-- cr eate a vertical divider here -->
        <div class="border-r-2 border-[#2F6A50] h-80"></div>
      </div>
      <div class="flex flex-col w-6/12 h-11/12 items-start">
        <div class="flex flex-row justify-start items-center w-full">
          <button onclick="window.location.href = '../index.php';"
            class="w-36 rounded-xl text-xl font-semibold text-[#1B4332] hover:cursor-pointer mb-5 text-left ml-2">
            <span><i class="fa-solid fa-angle-left"></i></span> Back</button>
        </div>
        <div class="flex flex-col w-full h-full p-12 rectBox">
          <div class="flex flex-row justify-between">
            <div class="flex flex-col">
              <h1 class="text-lg font-semibold text-[#1B4332] mb-4">
                Status of Complaint:
                <?php
                if ($check) {
                  if ($row['Status'] == 1) {
                    echo '<span class="text-[#2F6A50]">Completed</span>';
                  } else {
                    echo '<span class="text-[#2F6A50]">In Progress</span>';
                  }
                } else {
                  echo '<span class="text-[#2F6A50]">Complaint Not Found</span>';
                }
                ?>
              </h1>
            </div>
          </div>
          <!-- create horizontal divider-->
          <div class="border-b-2 border-[#2F6A50] w-full mt-10"></div>
          <div class="flex flex-col mt-10">
            <?php
            if ($check) {
              if ($row['Status'] == 1) {
                echo
                  '<h1 class="text-lg font-semibold text-[#1B4332] mb-4">
              Feedback:
            </h1>
            <h1 class="text-lg text-[#1B4332]">
              ';if(strlen(($row['Feedback']))<=2){
                echo 'Not Available';
              }else{
                echo $row['Feedback'];
              } echo '
            </h1>
            <div class="border-b-2 border-[#2F6A50] w-full mt-10"></div>
            ';
              }
              echo
                '
            <h1 class="text-lg font-semibold text-[#1B4332] mb-4">
              Complaint:
            </h1>
            <h1 class="text-lg text-[#1B4332]">
              ', $row['Complaint'], '
            </h1>
          ';
            }

            ?>
          </div>
        </div>
      </div>
    </div>
  </div>
</body>

</html>