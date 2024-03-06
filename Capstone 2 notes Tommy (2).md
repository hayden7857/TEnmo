Capstone 2 notes

document api
instructor can validate design
design from a service prospective: heavy work done on server side
you dont need alot of services if designed right
client should never know about others account number

server provides all functionality
server does all work get data and returns it
do serverside and test with postman then do client test


go in order as they are described

current ballance does not include pending
get request for balance use token for info

build all services on client menu in server

4 gives list of users probably need a function to get list of users. this one screws up alot of teams
    ui get amount and other user

    option 4 needs receiver and amount
    cant send 0 or negative amount

    sender goes in as an approved transfer
    insert into transfer table

    option 4 has 5 things to do on serverside 
        3 things if you are good with subselects

    updating balance for sender and receiver create transfer object
        send it to server and server do work. respond with message
        One call to server
        client send minimum amount possible
        {who you are, who are you sending it to, amount}
        dont send account numbers or user id as url

        lookup account number from user id

5 retrieve list of tansfers that include auth user
    fix where clause instead of two calls
    

6 retrieve details of specific transfer 
    tansfer id can go in url
    possibly another webservice or pull all and parse
    rauch said crud

7 request money as pending transfer
    show list of users
    cant request from self or 0 or neg amount
    {user id requester,user id sender, amount}
    possibly use same transfer object
    4 and 7 are similar kinda together as far as objects
    request has user id need to get account number(account_from,account_to) from account table subselect

8 see pending transfers and approve or reject transfer
    receiving money does not need approval
    
9 select from list then reject or approve


day one have end to end for account balance
    ideally onto 4

call userSqlDao from other daos get user from id or username
has get users already made

server side integration test for dao

best way to fail divide and conquer 
do not use client to test server until tested in postman
use debugger on serverside


Make dao do the work


