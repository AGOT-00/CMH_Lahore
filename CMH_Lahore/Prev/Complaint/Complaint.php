<!doctype html>
<html>

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="..\res\Tailwind.js"></script>
    <link rel="stylesheet" href="../res/Custom.css">

</head>

<body>
    <script src="https://kit.fontawesome.com/8ed36584a1.js" crossorigin="anonymous"></script>
    <?php
    //Supress all Warning in Php
    error_reporting(0);
    session_start();
    $_SESSION['dname'] = $_POST['dname'];
    $_SESSION['departname'] = $_POST['departname'];
    $_SESSION['Roomno'] = $_POST['Roomno'];
    $_SESSION['DateofIssue'] = $_POST['DateofIssue'];
    //echo $_SESSION['departname'];
    //$complaint = $_POST['complaint'];
    
    ?>
    <div class="flex flex-col w-screen h-screen justify-center items-center">
        <div class="flex flex-row justify-evenly items-center w-screen h-screen">
            <!-- <div class="w-2/12">
                <img src="..\res\img\logo.png">
            </div> -->

            <div class="flex flex-col w-4/12 h-11/12 items-center">
                <div class="topBar rounded-full w-3/12 p-2 flex flex-row justify-between items-center absolute -mt-8">
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <i class="fa-solid fa-check text-xl text-[#1B4332]"></i>
                    </div>
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <i class="fa-solid fa-check text-xl font-semibold text-[#1B4332]"></i>
                    </div>
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <h1 class="text-xl font-semibold text-[#1B4332]">3</h1>
                    </div>
                </div>

                <form action="./submissionComplete.php" method="POST" name="submitcompform"
                    class="flex flex-col w-full h-full p-12 rectBox">
                    <h1 class="text-2xl font-semibold text-[#2F6A50] mt-5">Complaint</h1>
                    <textarea placeholder="Enter Your complaint here"
                        class="w-full text-xl mt-10 p-5 bg-[#FAFFFA] rounded-lg placeholder-[#43916DCC] text-[#2F6A50]"
                        rows="10" name="complaint" id="complaint" minlength=30 required></textarea>
                    <input type="submit" value="Submit" name="submitcompform"
                        class="w-full text-xl mt-6 p-5 bg-[#2F6A50] text-[#D8F3DC] font-semibold rounded-lg text-[#2F6A50] hover:cursor-pointer">
                </form>

            </div>
            <div>
                <!-- create a vertical divider here -->
                <div class="border-r-2 border-[#2F6A50] h-80"></div>
            </div>
            <div class="flex flex-col w-4/12 h-11/12 items-center">
                <div class="topBar rounded-full w-3/12 p-2 flex flex-row justify-between items-center absolute -mt-8">
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <i class="fa-solid fa-check text-xl text-[#1B4332]"></i>
                    </div>
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <i class="fa-solid fa-check text-xl font-semibold text-[#1B4332]"></i>
                    </div>
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <h1 class="text-xl font-semibold text-[#1B4332]">3</h1>
                    </div>
                </div>

                <div class="flex flex-col w-full h-full p-12 rectBox">
                    <h1 class="text-2xl font-semibold text-[#2F6A50] mt-5">Record your Complaint</h1>
                    <div
                        class="controllers w-full text-center mt-4 text-xl p-2 bg-[#2F6A50] text-[#D8F3DC] font-semibold rounded-lg text-[#2F6A50] hover:cursor-pointer">
                    </div>
                    <div class="flex flex-col justify-evenly">
                        <div class="display w-full font-semibold mt-2 text-center items-center">

                        </div>
                        <button name="recsubmit" id="recsubmit"
                            class="w-full text-xl p-5 bg-[#2F6A50] text-[#D8F3DC] font-semibold rounded-lg text-[#2F6A50] hover:cursor-pointer">
                            Submit
                        </button>
                    </div>
                    <script src="../res/voice.js"></script>

                </div>

            </div>

        </div>
    </div>
</body>


</html>