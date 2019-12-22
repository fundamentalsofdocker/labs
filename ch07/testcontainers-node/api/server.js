const express = require('express');
const app = express();

const host = process.env.DB_HOST || 'localhost'
const port = process.env.DB_PORT || '5432'
console.log("Database is at: %s:%s", host,  port)

const { Pool } = require('pg')
const pool = new Pool({
    user: 'dbuser',
    host: host,
    database: 'sample-db',
    password: 'secretpassword',
    port: parseInt(port),
})

app.listen(3000, '0.0.0.0', () => {
    console.log('Application listening at 0.0.0.0:3000');
})

app.get('/hobbies', async (req, res) => {
    const result = await pool.query('SELECT hobby FROM hobbies')
    res.send(result.rows );
})

app.get('/hobbies/:id', async (req, res) => {
    const id = req.params.id;
    const result = await pool.query('SELECT hobby FROM hobbies WHERE hobby_id=$1', [id])
    res.send(result.rows[0].hobby );
})

app.get('/', (req, res) => {
    res.send('Sample API');
})
