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
    session_start();
    if (isset($_SESSION['Loggedin']) == false) {
        header('Location: ./');
    }
    include('./localres/Admins.php');
    $User = $_SESSION['UserObject'];

    $User = unserialize($User);

    $result = $User->getcomplaintdata(1);//mysqli_query($conn, $sql);

    ?>
    <script src="https://kit.fontawesome.com/8ed36584a1.js" crossorigin="anonymous"></script>
    <div class="flex flex-row w-screen h-screen">
        <aside class="w-1/5 h-screen bg-white">
            <div class="flex flex-col justify-center items-center h-1/4">
                <img src="../res/img/adminLogo.png" class="w-1/2" />
            </div>
            <div class="flex flex-col w-full justify-around items-center h-3/4">
                <div class="flex flex-col items-center w-full">
                    <div class="w-full text-xl font-semibold text-[#1B4345] hover:cursor-pointer mb-5 p-5 text-center">
                        Complaints
                    </div>
                    <button onclick="window.location.href = './Dashboard.php';"
                        class="w-full hover:bg-[#D8F3DC] text-xl font-semibold text-[#1B4332] mb-5 p-5 text-center">
                        Pending
                    </button>
                    <button onclick="window.location.href = './closedcomplaint.php';"
                        class="w-full bg-[#D8F3DC] text-xl font-semibold text-[#1B4332] hover:cursor-pointer mb-5 p-5 text-center">
                        Closed
                    </button>
                </div>
                <!-- logout button at bottom-->
                <div class="flex flex-col items-center w-full">
                    <button onclick="window.location.href = './localres/logout.php';"
                        class="w-3/6 rounded-xl text-xl hover:bg-[#2F6A50] font-semibold bg-[#1B4332] text-[#D8F3DC] hover:cursor-pointer p-5 text-center">
                        Logout
                    </button>
                </div>
        </aside>
        <!--create verttical divider with h-screen -->
        <div class="border-r-2 border-[#2F6A50] h-screen"></div>
        <div class="flex flex-col w-full px-24">
            <div class="overflow-x-auto relative mt-24">
                <table class="w-full font-semibold text-[#D8F3DC] bg-[#2F6A50] text-left">
                    <thead class="text-lg text-[#D8F3DC] bg-[#2F6A50]">
                        <tr>
                            <th scope="col" class="py-3 px-6 font-semibold">
                                Sr. Number
                            </th>
                            <th scope="col" class="py-3 px-6 font-semibold">
                                Name of the Complainant
                            </th>
                            <th scope="col" class="py-3 px-6 font-semibold">
                                Phone Number
                            </th>
                            <th scope="col" class="py-3 px-6 font-semibold">
                                Date of Complaint
                            </th>
                            <th scope="col" class="py-3 px-6 font-semibold">
                                Actions
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <?php
                        while ($row = $result->fetch_array()) {

                            echo '
                        <tr class="bg-[#D8F3DC] text-[#1B4332] border-b-2 border-[#1B4332]">
                            <td class="py-4 px-6 text-lg">
                                ', $row['ComplaintID'], '
                            </td>
                            <td class="py-4 px-6 text-lg">
                            ', $row['CName'], '
                            </td>
                            <td class="py-4 px-6 text-lg">
                            ', $row['Phone'], '
                            </td>
                            <td class="py-4 px-6 text-lg">
                            ', $row['DateofComplaint'], '
                            </td>
                            <td class="py-4 pl-6">
                            <form action="./viewcomplaint.php" method="POST">
                                <button id="view" name="view" type="submit" value="', $row['ComplaintID'], '"
                                    class="bg-[#2F6A50] text-[#D8F3DC] hover:bg-[#1B4332] hover:cursor-pointer p-2 rounded-xl">
                                    View
                                </button>
                            </form>
                            </td>
                        </tr>';
                        }
                        ?>
                    </tbody>
                </table>
            </div>

        </div>
    </div>
    <script>
        function getlogout() {

        }
    </script>
</body>

</html>