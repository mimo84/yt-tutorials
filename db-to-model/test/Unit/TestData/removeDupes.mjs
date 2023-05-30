import fs from 'fs'

const f = fs.readFileSync('./file input.csv', 'utf-8');
const rows = f.split('\n');

const out = Object.keys(rows.reduce((prev, curr) => {
  prev[curr] = curr;
  return prev;
}, {})).join('\n')

fs.writeFileSync('no-dups.csv', out, 'utf-8')
