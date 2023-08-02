var calendarTableElement = document.getElementById("calendar-table");
var gridTableElement = document.getElementById("table-body");

var todaysDate = new Date();
var currentDate = todaysDate;

var selectedDate = currentDate;
var selectedDayBlock = null;

var firstSelectedDate = null;
var secondSelectedDate = null;

var count = 0;

var prevButton = document.getElementById("prev");
var nextButton = document.getElementById("next");

var clearBtn = document.getElementById("clearBtn");

function createCalendar(date, side, startDateAsText, endDateAsText) {

    currentDate = date;

    if (startDateAsText != null && endDateAsText != null) {
        var startDate = new Date(startDateAsText);
        firstSelectedDate = startDate;
        currentDate = startDate;

        var endDate = new Date(endDateAsText);
        secondSelectedDate = endDate;
    }

    var monthTitle = document.getElementById("month-name");
    monthTitle.innerHTML = getMonthTitle();

    var todayDayName = document.getElementById("todayDayName");
    todayDayName.innerHTML = getTodaysTitle();

    addGridAnimation(side);

    setTimeout(() => {
        gridTableElement.innerHTML = "";

        var newTr = document.createElement("div");
        newTr.className = "row";

        var currentTr = gridTableElement.appendChild(newTr);

        createEmptyMonthStartCells();

        var lastDay = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
        lastDay = lastDay.getDate();

        createDayCels();
        createEmptyMonthEndCells();

        function createDayCels() {
            for (let i = 1; i <= lastDay; i++) {
                if (currentTr.children.length >= 7) {
                    currentTr = gridTableElement.appendChild(addNewRow());
                }

                let currentDayDiv = document.createElement("div");
                currentDayDiv.className = "col";

                colorTodaysDay();
                colorSelectedStartDay();
                colorSelectedEndDay();
                addCellDayNumber();

                currentTr.appendChild(currentDayDiv);

                function colorTodaysDay() {
                    if (selectedDayBlock == null && i == currentDate.getDate()
                        || selectedDate.toDateString() == new Date(currentDate.getFullYear(), currentDate.getMonth(), i).toDateString()) {
                        selectedDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), i);

                        selectedDayBlock = currentDayDiv;
                        setTimeout(() => {
                            currentDayDiv.classList.add("green");
                            currentDayDiv.classList.add("lighten-3");
                        }, 100);
                    }
                }

                function colorSelectedStartDay() {
                    if (firstSelectedDate != null && firstSelectedDate.getMonth() == currentDate.getMonth()
                        && firstSelectedDate.getDate() == i) {
                        setTimeout(() => {
                            currentDayDiv.classList.remove("green");
                            currentDayDiv.classList.add("blue");
                            currentDayDiv.classList.add("lighten-3");
                        }, 100);
                    }
                }

                function colorSelectedEndDay() {
                    if (secondSelectedDate != null && secondSelectedDate.getMonth() == currentDate.getMonth()
                        && secondSelectedDate.getDate() == i) {
                        setTimeout(() => {
                            currentDayDiv.classList.remove("green");
                            currentDayDiv.classList.remove("lighten-3");
                            currentDayDiv.classList.add("blue");
                            currentDayDiv.classList.add("lighten-5");
                        }, 100);
                    }
                }

                function addCellDayNumber() {
                    if ((currentDate.getMonth() == todaysDate.getMonth() && i < todaysDate.getDate())
                        || currentDate.getMonth() < todaysDate.getMonth()
                        || currentDate.getFullYear() < todaysDate.getFullYear()) {
                        currentDayDiv.innerHTML = `<s>${i}</s>`;
                    }
                    else {
                        currentDayDiv.innerHTML = i;
                    }
                }
            }
        }

        function createEmptyMonthStartCells() {
            var startDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
            for (let i = 1; i < (startDate.getDay() || 7); i++) {
                let emptyDivCol = document.createElement("div");
                emptyDivCol.className = "col empty-day";
                currentTr.appendChild(emptyDivCol);
            }
        }

        function addNewRow() {
            let node = document.createElement("div");
            node.className = "row";
            return node;
        }

         function createEmptyMonthEndCells() {
            for (let i = currentTr.getElementsByTagName("div").length; i < 7; i++) {
                let emptyDivCol = document.createElement("div");
                emptyDivCol.className = "col empty-day";
                currentTr.appendChild(emptyDivCol);
            }
        }

    }, !side ? 0 : 270);

    function getMonthTitle() {

        var monthName = currentDate.toLocaleString("en-US", {
            month: "long"
        });
        var yearNum = currentDate.toLocaleString("en-US", {
            year: "numeric"
        });

        return `${monthName} ${yearNum}`;
    }

    function getTodaysTitle() {
        return "Today is " + todaysDate.toLocaleString("en-US", {
            weekday: "long",
            day: "numeric",
            month: "short"
        });
    }

    function addGridAnimation(side) {
        if (side == "left") {
            gridTableElement.className = "animated fadeOutRight";
            gridTableElement.className = "animated fadeInLeft";
        } else {
            gridTableElement.className = "animated fadeOutLeft";

            gridTableElement.className = "animated fadeInRight";
        }
    }
}

prevButton.onclick = function changeMonthPrev() {
    currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1);
    createCalendar(currentDate, "left");
}

nextButton.onclick = function changeMonthNext() {
    currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1);
    createCalendar(currentDate, "right");
}

gridTableElement.onclick = function (e) {

    if (!e.target.classList.contains("col") || e.target.classList.contains("empty-day")) {
        return;
    }

    var currentYear = currentDate.getFullYear();
    var currentMonth = currentDate.getMonth();
    var currentDayText = e.target.innerHTML;

    if (currentDayText.indexOf("<s>") !== -1) {
        return;
    }

    if (firstSelectedDate != null && secondSelectedDate != null) {
        return;
    }

    if (count % 2 == 0) {

        if (selectedDayBlock) {
            if (selectedDayBlock.classList.contains("green") && selectedDayBlock.classList.contains("lighten-3")) {
                selectedDayBlock.classList.remove("green");
                selectedDayBlock.classList.remove("lighten-3");
            }
        }
        selectedDayBlock = e.target;
        selectedDayBlock.classList.add("blue");
        selectedDayBlock.classList.add("lighten-3");

        selectedDate = new Date(currentYear, currentMonth, parseInt(currentDayText));
        firstSelectedDate = selectedDate;

        var startDayElement = document.getElementById("startDay");
        startDayElement.value = getDateAsString(selectedDate);
    }
    else {
        if (selectedDayBlock) {
            if (selectedDayBlock.classList.contains("green")) {
                selectedDayBlock.classList.remove("green");
            }

            if (selectedDayBlock.classList.contains("lighten-3")) {
                selectedDayBlock.classList.remove("lighten-3");
            }
        }
        selectedDayBlock = e.target;
        selectedDayBlock.classList.add("blue");
        selectedDayBlock.classList.add("lighten-5");

        selectedDate = new Date(currentYear, currentMonth, parseInt(currentDayText));
        secondSelectedDate = selectedDate;

        var endDayElement = document.getElementById("endDay");
        endDayElement.value = getDateAsString(selectedDate);
    }

    count++;
}

clearBtn.onclick = function clearSelectedDates() {
    firstSelectedDate = null;
    secondSelectedDate = null;
    currentDate = new Date();
    selectedDate = currentDate;
    document.getElementById("startDay").value = getDateAsString(todaysDate);
    document.getElementById("endDay").value = getDateAsString(todaysDate);
    createCalendar(currentDate, "right");
}

function getDateAsString(selectedDate) {
    var day = selectedDate.getDate();
    var month = selectedDate.getMonth();
    var year = selectedDate.getFullYear();

    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    return day + " " + monthNames[month] + " " + year; 
}