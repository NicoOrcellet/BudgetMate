
const balanceSelect = document.getElementById('balanceSelection')
const monthBalance = document.getElementById('monthBalance')
const weekBalance = document.getElementById('weekBalance')
const incExpSelect = document.getElementById('incomeExpenseSelection')
const monthIncExp = document.querySelectorAll('.monthIncomeExpense')
const weekIncExp = document.querySelectorAll('.weekIncomeExpense')
const monthProgress = document.getElementById('monthProgress')
const weekProgress = document.getElementById('weekProgress')
const savingLimitSelection = document.getElementById('savingLimitSelection')
const progressBar = document.getElementById("progressBar")

if (!localStorage.getItem("userId")) {
    const userId = 1
    localStorage.setItem("userId", userId)
}

const userId = localStorage.getItem('userId');
const date = new Date();
date.setTime(date.getTime() + (7 * 24 * 60 * 60 * 1000));
document.cookie = `userId=${userId};expires=${date.toUTCString()};path=/`;

balanceSelect.addEventListener('change', (event) => {
    if (event.target.value == 'month') {
        monthBalance.style.display = 'block'
        weekBalance.style.display = 'none'
    } else {
        weekBalance.style.display = 'block'
        monthBalance.style.display = 'none'
    }
})

incExpSelect.addEventListener('change', (event) => {
    if (event.target.value == 'month') {
        monthIncExp.forEach(element => {
            element.style.display = 'block'
        })
        weekIncExp.forEach(element => {
            element.style.display = 'none'
        })
    } else {
        monthIncExp.forEach(element => {
            element.style.display = 'none'
        })
        weekIncExp.forEach(element => {
            element.style.display = 'block'
        })
    }
})

function checkPercentageProgress(percentage) {
    if (percentage > 100) {
        document.getElementById("progressBar").style.width = "100%"
        document.getElementById("progressBar").style.background = "radial-gradient(#ff2323,#b10303)"
        document.getElementById("progressBar").style.textShadow = "#920a0a 2px 2px 2px"
        document.getElementById("progressBar").textContent = "¡" + percentage + "%!"
    } else {
        document.getElementById("progressBar").style.background = "linear-gradient(90deg, orangered 90%, #ff8232)"
        document.getElementById("progressBar").style.textShadow = "#c63408 2px 2px 2px"
        document.getElementById("progressBar").style.width = percentage + "%"
        document.getElementById("progressBar").textContent = percentage + "%"
    }
}

function updateProgressBar(period) {
    if (period == 'month') {
        if (monthProgress.getAttribute('data-exists') == "False") {
            progressBar.style.display = 'none'
            document.querySelector('.progress-container').style.display = 'none'
        } else {
            progressBar.style.display = 'block'
            document.querySelector('.progress-container').style.display = 'block'
            setTimeout(() => {
                const percentage = (document.getElementById("monthExpense").textContent.replace(',', '.') * 100) / document.getElementById("monthLimit").textContent.replace(',', '.')
                checkPercentageProgress(percentage)
            }, 100)
        }
    } else {
        if (weekProgress.getAttribute('data-exists') == "False") {
            document.querySelector('.progress-container').style.display = 'none'
            progressBar.style.display = 'none'
        } else {
            document.querySelector('.progress-container').style.display = 'block'
            progressBar.style.display = 'block'
            setTimeout(() => {
                const percentage = (document.getElementById("weekExpense").textContent.replace(',', '.') * 100) / document.getElementById("weekLimit").textContent.replace(',', '.')
                checkPercentageProgress(percentage)
            }, 100)
        }
    }  
}

updateProgressBar('month')

savingLimitSelection.addEventListener('change', (event) => {
    if (event.target.value == 'month') {
        monthProgress.style.display = 'block'
        weekProgress.style.display = 'none'
        updateProgressBar(event.target.value)
    } else {
        monthProgress.style.display = 'none'
        weekProgress.style.display = 'block'
        updateProgressBar(event.target.value)
    }
})