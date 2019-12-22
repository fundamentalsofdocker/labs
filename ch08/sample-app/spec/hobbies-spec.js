const sut = require('../hobbies');

describe("Hobbies suite", () => {
    describe("GET /getHobbies", () => {
        const hobbies = sut.getHobbies();
        it("should return 5 hobbies", () => {
            expect(hobbies.length).toBe(5);     
        })
    })
    describe("GET /getHobby", () => {
        it("should return swimming as hobby with ID=1", () => {
            const hobby = sut.getHobby(1);
            expect(hobby).toBe("swimming");
        })
        it("should return 'undefined' for ID<1", () => {
            const hobby = sut.getHobby(-1);
            expect(hobby).toBeUndefined();
        })
    })
})