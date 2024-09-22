package ec.edu.espol.kokoa.trivia.ui.viewmodels

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import ec.edu.espol.kokoa.trivia.domain.model.Topic
import ec.edu.espol.kokoa.trivia.domain.repository.TriviaRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class TopicsViewModel @Inject constructor(
    trivia: TriviaRepository
) : ViewModel() {

    private val _topics: MutableStateFlow<List<Topic>> = MutableStateFlow(emptyList())
    val topics: StateFlow<List<Topic>> = _topics;

    private val _loading: MutableStateFlow<Boolean> = MutableStateFlow(false)
    val loading: StateFlow<Boolean> = _loading;

    init {
        viewModelScope.launch {
            _loading.update { true }
            _topics.update { trivia.getTopics() }
            _loading.update { false }
        }
    }

}