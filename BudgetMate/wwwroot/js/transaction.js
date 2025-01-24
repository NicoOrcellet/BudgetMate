const searchSelect = document.getElementById('searchSelect')
const dateInput = document.getElementById('dateInput')
const amountInput = document.getElementById('amountInput')
const categoryInput = document.getElementById('categoryInput')
const cleanFilters = document.getElementById('cleanFilters')
const searchButton = document.getElementById('searchButton')
const inputs = document.querySelectorAll('input')
const form = document.querySelector('form')

searchSelect.addEventListener('change', (event) => {
    switch (event.target.value) {
        case 'amount':
            dateInput.style.display = 'none'
            categoryInput.style.display = 'none'
            amountInput.style.display = 'block'
            break;
        case 'date':
            dateInput.style.display = 'flex'
            categoryInput.style.display = 'none'
            amountInput.style.display = 'none'
            break;
        case 'category':
            dateInput.style.display = 'none'
            categoryInput.style.display = 'block'
            amountInput.style.display = 'none'
            break;
    }
    cleanFilters.style.display = 'block'
    searchButton.style.display = 'block'
})

cleanFilters.addEventListener('click', (event) => {
    dateInput.style.display = 'none'
    categoryInput.style.display = 'none'
    amountInput.style.display = 'none'
    cleanFilters.style.display = 'none'
    searchButton.style.display = 'none'
    searchSelect.value = 'none'
    form.submit();
})