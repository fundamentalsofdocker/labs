const hobbies = ["swimming", "dancing", "jogging", "cooking", "diving"];

exports.getHobbies = () => {
    return hobbies;
}

exports.getHobby = id => {
    return hobbies[id-1];
}