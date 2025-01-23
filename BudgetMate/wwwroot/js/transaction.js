
const searchSelect = document.getElementById('searchSelect')
const dateInput = document.getElementById('dateInput')
const amountInput = document.getElementById('amountInput')
const categoryInput = document.getElementById('categoryInput')
const cleanFilters = document.getElementById('cleanFilters')
const searchButton = document.getElementById('searchButton')

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

cleanFilters.addEventListener('click', () => {
    (event) => {
        searchSelect.target.value = 'none'
        dateInput.style.display = 'none'
        categoryInput.style.display = 'none'
        amountInput.style.display = 'none'
        cleanFilters.style.display = 'none'
        searchButton.style.display = 'none'
    }})