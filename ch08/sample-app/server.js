const express = require('express');
const app = express();
const hobbies = require('./hobbies');

app.listen(3000, '0.0.0.0', () => {
    console.log('Application listening at 0.0.0.0:3000');
})

app.get("/hobbies", (req,res) => {
    res.send(hobbies.getHobbies());
})
