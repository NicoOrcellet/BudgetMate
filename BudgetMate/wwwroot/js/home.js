
const balanceSelect = document.getElementById('balanceSelection')
const monthBalance = document.getElementById('monthBalance')
const weekBalance = document.getElementById('weekBalance')
const incExpSelect = document.getElementById('incomeExpenseSelection')
const monthIncExp = document.querySelectorAll('.monthIncomeExpense')
const weekIncExp = document.querySelectorAll('.weekIncomeExpense')

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