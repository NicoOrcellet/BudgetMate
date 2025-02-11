
const balanceSelect = document.getElementById('balanceSelection')
const monthBalance = document.getElementById('monthBalance')
const weekBalance = document.getElementById('weekBalance')
const incExpSelect = document.getElementById('incomeExpenseSelection')
const monthIncExp = document.querySelectorAll('.monthIncomeExpense')
const weekIncExp = document.querySelectorAll('.weekIncomeExpense')

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
        monthBalance.style.display = 'block';
        weekBalance.style.display = 'none';
    } else {
        weekBalance.style.display = 'block';
        monthBalance.style.display = 'none';
    }
})

incExpSelect.addEventListener('change', (event) => {
    if (event.target.value == 'month') {
        monthIncExp.forEach(element => {
            element.style.display = 'block';
        })
        weekIncExp.forEach(element => {
            element.style.display = 'none';
        })
    } else {
        monthIncExp.forEach(element => {
            element.style.display = 'none';
        })
        weekIncExp.forEach(element => {
            element.style.display = 'block';
        })
    }
})