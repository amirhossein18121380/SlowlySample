
    $(document).ready(function () {
        $('#previewProfileBtn').on('click', function () {
            $.ajax({
                url: '/Profile/GetUserProfilePreviewQuery',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    console.log(data)
                    updateProfileModal(data);
                },
                error: function (error) {
                    console.error('Error fetching profile data:', error);
                }
            });
        });

        function updateProfileModal(profileData) {
            // Update modal content with the received data
            $('#modalDisplayName').text(profileData.displayName);
            $('#modalAvatar').attr('src', profileData.avatarUrl);
            $('#modalDisplayNameAbout').text(profileData.displayName);
            $('#modalAbout').text(profileData.aboutMe);

            // Update other profile details
            $('#modalBirthDate').html(profileData.dateOfBirth);
            
            var genderType = profileData.genderType;
            var genderIconClass = getGenderTypeIcon(genderType);
            var genderLabel = getGenderTypeLabel(genderType);

            $('#modalGenderIcon').html(`<i class="${genderIconClass}"></i>`);
            $('#modalGender').text(`Gender: ${genderLabel}`);

            //$('#modalGender').text(`Gender: ${getGenderTypeLabel(profileData.genderType)}`);
            $('#modalCountry').text(profileData.country);

            // Writing Habits
            $('#modalLetterLength').html(`Letter Length: <strong>${getLetterLengthLabel(profileData.letterLength)}</strong>`);
            $('#modalReplyTime').html(`Reply Time: <strong>${getReplyTimeLabel(profileData.replyTime)}</strong>`);

            // Topics
            var topicsHtml = '';
            if (profileData.topics && profileData.topics.length > 0) {
                topicsHtml = profileData.topics.map(topic => `<span class="badge badge-pill badge-tag badge-active">${topic}</span>`).join('');
            }
            $('#modalTopics').html(topicsHtml);

            // Languages
            var languagesHtml = '';
            if (profileData.languages && profileData.languages.length > 0) {
                languagesHtml = profileData.languages.map(language => {
                    // Check if the properties exist before accessing them
                    const name = language.name || 'Unknown Language';
                    const level = language.level || 0;
                    return `
                            <div class="col-6 mb-2">${name}
                                <div class="small lang-level">${getLanguageLevelHtml(level)}</div>
                            </div>`;
                }).join('');
            }
            $('#modalLanguages').html(languagesHtml);

            // Show the modal
            $('#profileModal').modal('show');
        }

        function getLetterLengthLabel(letterLength) {
            switch (letterLength) {
                case 0: return 'No Preferences';
                case 1: return 'Short';
                case 2: return 'Short-Medium';
                case 3: return 'Medium';
                case 4: return 'Medium-Long';
                case 5: return 'Long';
                default: return 'Unknown';
            }
        }

        function getReplyTimeLabel(replyTime) {
            switch (replyTime) {
                case 0: return 'As Soon As Possible';
                case 1: return 'Within A Week';
                case 2: return 'Within 2 Weeks';
                case 3: return 'Within 3 Weeks';
                case 4: return 'Within A Month';
                case 5: return 'Over A Month';
                default: return 'Unknown';
            }
        }

        function getGenderTypeLabel(genderType) {
            switch (genderType) {
                case 0: return 'Man';
                case 1: return 'Woman';
                default: return 'Unknown';
            }
        }
        
        function getGenderTypeIcon(genderType) {
            switch (genderType) {
                case 0: return 'bi bi-gender-male text-calm'; // Male
                case 1: return 'bi bi-gender-female text-calm'; // Female
                default: return 'bi bi-question text-danger'; // Unknown or other
            }
        }


        function getLanguageLevelHtml(level) {
            var totalSquares = 6;
            var levelHtml = '';

            for (var i = 1; i <= totalSquares; i++) {
                var isFilled = i <= level;
                var iconClass = isFilled ? 'bi bi-check-square text-calm' : 'bi bi-square text-calm';

                levelHtml += `<i class="${iconClass}"></i>`;
            }

            return levelHtml;
        }

        ///////////



    });

function saveProfile() {
    // Gather form data
    var letterLength = $("#LetterLength").val();
    var replyTime = $("#ReplyTime").val();
    var aboutMe = $("#AboutMe").val();
    var allowAddMeById = $("#add_by_id").is(":checked");

    var requestData = {
        LetterLength: letterLength,
        ReplyTime: replyTime,
        AboutMe: aboutMe,
        AllowAddMeById: allowAddMeById
    };

    console.log(JSON.stringify(requestData))
    $.ajax({
        url: '@Url.Action("UpdateProfile", "Profile")',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(requestData),
        success: function (response) {
            if (response.statusCode === 200) {
                showSuccessAlert(response.message, { autoClose: true, timeout: 3000 });
            }
        },
        error: function (error) {
            showErrorAlert(error, { autoClose: true, timeout: 3000 });
        }
    });
}