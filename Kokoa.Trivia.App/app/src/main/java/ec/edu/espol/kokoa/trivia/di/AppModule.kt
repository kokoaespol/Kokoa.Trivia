package ec.edu.espol.kokoa.trivia.di

import com.squareup.moshi.Moshi
import com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.components.SingletonComponent
import ec.edu.espol.kokoa.trivia.data.remote.TriviaApi
import ec.edu.espol.kokoa.trivia.data.repository.TriviaRepositoryImpl
import ec.edu.espol.kokoa.trivia.domain.repository.TriviaRepository
import retrofit2.Retrofit
import retrofit2.converter.moshi.MoshiConverterFactory
import javax.inject.Singleton

@Module
@InstallIn(SingletonComponent::class)
object AppModule {

    @Provides
    @Singleton
    fun providesTriviaApi(): TriviaApi =
        Retrofit.Builder()
            .baseUrl("https://kokoa.espol.edu.ec/trivia/")
            .addConverterFactory(
                MoshiConverterFactory.create(
                    Moshi.Builder().add(KotlinJsonAdapterFactory()).build()
                )
            )
            .build()
            .create(TriviaApi::class.java)


    @Provides
    @Singleton
    fun providesTriviaRepository(api: TriviaApi): TriviaRepository = TriviaRepositoryImpl(
        api
    )

}