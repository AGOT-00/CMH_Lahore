<!DOCTYPE html>
<html>

<body>
    <div class="flex flex-row w-screen h-screen">
        <div class="flex flex-col w-full h-full items-start px-32 py-12">
            <div class="flex flex-row justify-start items-center w-full">
                <button onclick="window.location.href = './Dashboard.php';"
                    class="w-36 rounded-xl text-xl font-semibold text-[#1B4332] hover:cursor-pointer mb-5 text-left ml-2">
                    <span><i class="fa-solid fa-angle-left"></i></span>
                    Back</button>
            </div>
            <div class="flex flex-col w-full h-full p-12 rectBox">
                <div class="flex flex-row justify-between">
                    <div class="flex flex-col">
                        <h1 class="text-lg font-semibold text-[#1B4332] mb-4">
                            Complaintent Details:
                        </h1>
                        <h1 class="text-lg text-[#1B4332]">
                            Name: <span>
                                <?php echo $row['CName']; ?>
                            </span>
                        </h1>
                        <h1 class="text-lg text-[#1B4332]">
                            Phone Number:
                            <span>
                                <?php echo $row['Phone']; ?>
                            </span>
                        </h1>
                        <h1 class="text-lg text-[#1B4332]">
                            Date: <span>
                                <?php echo $row['DateofComplaint']; ?>
                            </span>
                        </h1>
                        <h1 class="text-lg text-[#1B4332]">
                            CNIC:
                            <span>
                                <?php echo $row['Ccnic']; ?>
                            </span>
                        </h1>
                    </div>
                    <div class="flex flex-col">
                        <h1 class="text-lg font-semibold text-[#1B4332] mb-4">
                            Complaintent Against:
                        </h1>
                        <h1 class="text-lg text-[#1B4332]">
                            Name: <span>
                                <?php echo $row['Docname']; ?>
                            </span>
                        </h1>
                        <h1 class="text-lg text-[#1B4332]">
                            Department Name:
                            <span>
                                <?php echo $departmentname; ?>
                            </span>
                        </h1>
                        <h1 class="text-lg text-[#1B4332]">
                            Room Number: <span>
                                <?php echo $row['RoomNo']; ?>
                            </span>
                        </h1>
                        <h1 class="text-lg text-[#1B4332]">
                            Date: <span>
                                <?php echo $row['DateofIssue']; ?>
                            </span>
                        </h1>
                    </div>
                    <div class="flex flex-col">
                        <h1 class="text-lg font-semibold text-[#1B4332] mb-4">
                            Action
                            <?php
                            if ($row['Status'] == 1) {
                                echo ' (Already Closed)';
                            } else {
                                echo ' (Already Open)';
                            }
                            ?>
                        </h1>
                        <div class="text-lg text-[#1B4332]">
                            <div class="flex flex-col gap-2">

                                <?php
                                if ($row['Status'] == 1) {
                                    echo '
                                    <form action="./localres/opencomplain.php" method="post">
                                <button type="submit"
                                    class="bg-[#2F6A50] text-[#D8F3DC] hover:bg-[#1B4332] hover:cursor-pointer px-5 py-3 rounded-xl">
                                    Add to Pending

                                </button></form>';
                                } else {
                                    echo '
                                    <form action="./localres/closecomplain.php" method="post">
                                <button type="submit"
                                    class="bg-[#2F6A50] text-[#D8F3DC] hover:bg-[#1B4332] hover:cursor-pointer px-5 py-3 rounded-xl">
                                    Close
                                </button></form>';
                                }
                                ?>

                            </div>
                        </div>
                    </div>
                </div>
                <!-- create horizontal divider-->
                <div class="border-b-2 border-[#2F6A50] w-full mt-10">
                </div>
                <div class="flex flex-col mt-10">
                    <h1 class="text-lg font-semibold text-[#1B4332] mb-4">
                        Complaint:
                    </h1>
                    <h1 class="text-lg text-[#1B4332]">
                        <span>

                            <?php if ($checkvoice) {
                            echo '<audio controls>
                                <source src=' . $voicepath . ' type="audio/mp3">
                            </audio>';
                        } else {
                            echo $row['Complaint'];
                        } ?>
                        </span>
                    </h1>
                </div>
            </div>
        </div>
    </div>
    </div>
</body>
</html>