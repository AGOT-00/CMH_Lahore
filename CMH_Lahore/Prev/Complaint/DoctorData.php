<!doctype html>
<html>

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="..\res\Tailwind.js"></script>
    <link rel="stylesheet" href="../res/Custom.css">
    <style>
        input::placeholder {
            text-align: right;
        }
    </style>
</head>

<body>
    <?php

    include('./functions.php');
    $departments = getDepartments();

    session_start();
    $_SESSION['cname'] = $_POST['cname'];
    $_SESSION['phone'] = $_POST['phone'];
    $_SESSION['cnic'] = $_POST['cnic'];

    ?>
    <script src="https://kit.fontawesome.com/8ed36584a1.js" crossorigin="anonymous"></script>
    <div class="flex flex-col w-screen h-screen justify-center items-center">
        <div class="flex flex-row justify-evenly items-center w-screen h-screen">
            <div class="w-2/12">
                <img src="..\res\img\logo.png">
            </div>
            <div>
                <!-- cr eate a vertical divider here -->
                <div class="border-r-2 border-[#2F6A50] h-80"></div>
            </div>
            <div class="flex flex-col w-4/12 h-11/12 items-center">
                <div class="topBar rounded-full w-3/12 p-2 flex flex-row justify-between items-center absolute -mt-8">
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <i class="fa-solid fa-check text-xl font-semibold text-[#1B4332]"></i>
                    </div>
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <h1 class="text-xl font-semibold text-[#1B4332]">2</h1>
                    </div>
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <h1 class="text-xl font-semibold text-[#1B4332]">3</h1>
                    </div>
                </div>
                <form action="./Complaint.php" method="POST" class="flex flex-col w-full h-full p-12 rectBox">

                    <h1 class="text-2xl font-semibold text-[#2F6A50] mt-5">Complaint Against</h1>

                    <div class="flex flex-col mt-4">
                        <h1 class="w-full">Doctor/Staff Name</h1>
                        <input type="text" placeholder="ڈاکٹر کا نام" name="dname" id="dname" minlength="3"
                            class="w-full text-xl p-5 bg-[#FAFFFA] rounded-lg placeholder-[#43916DCC] text-[#2F6A50]"
                            required>
                    </div>

                    <div class="flex flex-col mt-4">
                        <h1 class="w-full">Chose Department</h1>
                        <select name="departname" id="departname" placeholder="Department Name" style="direction: rtl;"
                            onchange="validateselectdepart()"
                            class="w-full text-[#2F6A50] text-xl p-5 bg-[#FAFFFA] placeholder-[#43916DCC] rounded-lg text-[#2F6A50]"
                            required>
                            <option value="-1">ڈیپارٹمنٹ</option>
                            <?php
                            while ($row = mysqli_fetch_assoc($departments)) {
                                echo "<option value='" . $row['DNumber'] . "'>" . $row['DName'] . "</option>";
                            }
                            ?>
                        </select>
                    </div>

                    <div class="flex flex-col mt-4">
                        <h1 class="w-full">Room Number (if possible)</h1>
                        <input type="text" placeholder="کمرہ نمبر" name="Roomno" id="Roomno" minlength="1"
                            onchange="validateselectdepart()"
                            class="w-full text-xl p-5 bg-[#FAFFFA] placeholder-[#43916DCC] rounded-lg text-[#2F6A50]"
                            >
                    </div>


                    <div class="flex flex-col mt-4">
                        <div class="flex flex-row">
                            <h1 class="w-full " style="text-align:left;">Date of Issue</h1>
                            <h1 class="w-full" style="text-align:right">شکایت کی تاریخ</h1>
                        </div>
                        <input type="datetime-local" name="DateofIssue" id="DateofIssue" placeholder="شکایت کی تاریخ"
                            min="2018-00-00T00:00" max="2030-06-14T00:00"
                            class="appearance-none text-xl w-full p-5 bg-[#FAFFFA] placeholder-[#43916DCC] rounded-lg"
                            required>
                    </div>


                    <input type="submit" value="Continue"
                        class="w-full text-xl mt-12 p-5 bg-[#2F6A50] text-[#D8F3DC] font-semibold rounded-lg text-[#2F6A50] hover:cursor-pointer">
                </form>
            </div>
        </div>
    </div>
</body>
<script>
    function validateselectdepart() {
        var selection = document.getElementById('departname');
        console.log('.'+selection.value+'.');
        if (selection.value == "-1") {
            alert('Kindly Select the Department First.\n برائے مہربانی پہلے محکمہ کا انتخاب کریں۔\n');
        }
    }
</script>

</html>