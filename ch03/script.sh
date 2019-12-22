while : 
do 
    wget -qO- http://jservice.io/api/random | jq .[0].question 
    sleep 5 
done
