package ec.edu.espol.kokoa.trivia.data.dto

import com.squareup.moshi.Json

data class QuestionDto(
    val id: Int,
    val title: String,
    @Json(name="correct_option") val correctOption: OptionDto,
    val options: List<OptionDto>
)
