package ec.edu.espol.kokoa.trivia.ui.viewmodels

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import ec.edu.espol.kokoa.trivia.domain.model.Question
import ec.edu.espol.kokoa.trivia.domain.repository.TriviaRepository
import kotlinx.coroutines.channels.Channel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.receiveAsFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class QuestionsViewModel @Inject constructor(
    val trivia: TriviaRepository
) : ViewModel() {

    private val _questions = MutableStateFlow<List<Question>>(emptyList())
    val questions: StateFlow<List<Question>> = _questions;

    private val _correctOptions = MutableStateFlow(mapOf<Int, Int>())
    val correctOptions: StateFlow<Map<Int, Int>> = _correctOptions

    private val _chosenOptions = MutableStateFlow(mapOf<Int, Int>())
    val chosenOptions: StateFlow<Map<Int, Int>> = _chosenOptions

    private val _incorrectAnswers = MutableStateFlow(setOf<Int>())
    val incorrectAnswers: StateFlow<Set<Int>> = _incorrectAnswers;

    private val _correctAnswers = MutableStateFlow(setOf<Int>())
    val correctAnswers: StateFlow<Set<Int>> = _correctAnswers

    private val _messages = Channel<String>()
    val messages = _messages.receiveAsFlow()

    fun loadQuestions(topicId: Int) = viewModelScope.launch {
        val questions = trivia.getQuestions(topicId)

        _questions.update {
            questions
        }

        _correctOptions.update {
            mutableMapOf(
                *questions.map { question ->
                    question.id to question.correctOption.id
                }.toTypedArray()
            )
        }
    }

    fun chooseOption(questionId: Int, optionId: Int) {
        _chosenOptions.update {
            it + (questionId to optionId)
        }
    }

    fun verifyAnswers() {
        if (_chosenOptions.value.size != _correctOptions.value.size) {
            viewModelScope.launch {
                _messages.send(
                    "Debe contestar todas las preguntas"
                )
            }
        } else {
            _incorrectAnswers.update {
                _chosenOptions.value.entries.fold(setOf<Int>()) { s, (question, option) ->
                    if (_correctOptions.value[question] != option) {
                        s + option
                    } else {
                        s
                    }
                }
            }

            _correctAnswers.update {
                _chosenOptions.value.entries.fold(setOf<Int>()) { s, (question, option) ->
                    if (_correctOptions.value[question] == option) {
                        s + option
                    } else {
                        s
                    }
                }
            }

            viewModelScope.launch {
                _messages.send(
                    if (_incorrectAnswers.value.isEmpty()) {
                        "Felicidades! Acertaste todas las preguntas"
                    } else {
                        "Te equivocaste en algunas preguntas"
                    }
                )
            }
        }
    }
}