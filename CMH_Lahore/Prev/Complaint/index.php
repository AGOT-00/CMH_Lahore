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
                        <h1 class="text-xl font-semibold text-[#1B4332]">1</h1>
                    </div>
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <h1 class="text-xl font-semibold text-[#1B4332]">2</h1>
                    </div>
                    <div class="flex flex-row w-12 h-12 rounded-full bg-[#D8F3DC] justify-center items-center">
                        <h1 class="text-xl font-semibold text-[#1B4332]">3</h1>
                    </div>
                </div>
                <form action="./DoctorData.php" method="POST" class="flex flex-col w-full h-full p-12 rectBox">
                    <h1 class="text-2xl font-semibold text-[#2F6A50] mt-5">Complaintent Information</h1>
                    <div class="flex flex-col mt-4">
                        <h1 class="w-full">Name of Complaintent</h1>
                        <input type="text" placeholder="شکایت  کنندہ  کا  نام" name="cname" id="cname" minlength=3
                            class="w-full text-xl p-5 bg-[#FAFFFA] rounded-lg placeholder-[#43916DCC] text-[#2F6A50]"
                            required>
                    </div>

                    <div class="flex flex-col mt-4">
                        <h1 class="w-full">Mobile Number</h1>
                        <input type="number" placeholder="موبائل  نمبر" name="phone" id="phone" minlength="10"
                            maxlength="15" required
                            class="w-full text-xl p-5 bg-[#FAFFFA] placeholder-[#43916DCC] rounded-lg text-[#2F6A50]">
                    </div>

                    <div class="flex flex-col mt-4">
                        <h1 class="w-full">CNIC of Complaintent</h1>
                        <input type="text" placeholder="شکایت کنندہ کا شناختی کارڈ نمبر" name="cnic" id="cnic"
                            class="w-full text-[#2F6A50] text-xl p-5 bg-[#FAFFFA] placeholder-[#43916DCC] rounded-lg text-[#2F6A50]">
                            <!-- required> -->
                    </div>


                    <input type="submit" value="Continue"
                        class="w-full text-xl mt-12 p-5 bg-[#2F6A50] text-[#D8F3DC] font-semibold rounded-lg text-[#2F6A50] hover:cursor-pointer">
                </form>
            </div>
        </div>
    </div>
</body>

</html>