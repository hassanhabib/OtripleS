﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------
using FluentAssertions;
using OtripleS.Web.Api.Models.Classrooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Classrooms
{
    public partial class ClassroomApiTests
    {
        [Fact]
        public async Task ShouldPostClassroomAsync()
        {
            // given
            Classroom randomClassroom = CreateRandumClassroom();
            Classroom inputClassroom = randomClassroom;
            Classroom expectedClassroom = inputClassroom;

            // when 
            await this.otripleSApiBroker.PostClassroomAsync(inputClassroom);

            Classroom actualClassroom =
                await this.otripleSApiBroker.GetClassroomByIdAsync(inputClassroom.Id);

            // then
            actualClassroom.Should().BeEquivalentTo(expectedClassroom);

            await this.otripleSApiBroker.DeleteClassroomByIdAsync(actualClassroom.Id);
        }

        [Fact]
        public async Task ShouldPutClassroomAsync()
        {
            // given
            Classroom randomClassroom = CreateRandumClassroom();
            await this.otripleSApiBroker.PostClassroomAsync(randomClassroom);
            Classroom modifiedClassroom = UpdateClassroomRandom(randomClassroom);

            // when
            await this.otripleSApiBroker.PutClassroomAsync(modifiedClassroom);

            Classroom actualClassroom =
                await this.otripleSApiBroker.GetClassroomByIdAsync(randomClassroom.Id);

            // then
            actualClassroom.Should().BeEquivalentTo(modifiedClassroom);

            await this.otripleSApiBroker.DeleteClassroomByIdAsync(actualClassroom.Id);
        }


        [Fact]
        public async Task ShouldGetAllClassroomsAsync()
        {
            // given
            IEnumerable<Classroom> randomClassrooms = GetRandomClassrooms();
            IEnumerable<Classroom> inputClassrooms = randomClassrooms;

            foreach (Classroom classroom in inputClassrooms)
            {
                await this.otripleSApiBroker.PostClassroomAsync(classroom);
            }

            List<Classroom> expectedClassrooms = inputClassrooms.ToList();

            // when
            List<Classroom> actualClassrooms = 
                await this.otripleSApiBroker.GetAllClassroomAsync();

            // then
            foreach (Classroom expectedClassroom in expectedClassrooms)
            {
                Classroom actualClassroom = 
                    actualClassrooms.Single(classroom => classroom.Id == expectedClassroom.Id);
                actualClassroom.Should().BeEquivalentTo(expectedClassroom);
                await this.otripleSApiBroker.DeleteClassroomByIdAsync(actualClassroom.Id);
            }
        }
    }
}
