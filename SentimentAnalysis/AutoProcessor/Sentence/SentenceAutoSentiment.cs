using System;
using System.Collections.Generic;
using AnnotatedSentence;
using SentiNet;

namespace SentimentAnalysis.AutoProcessor.Sentence {
    
    public abstract class SentenceAutoSentiment {
        
        protected readonly SentiNet.SentiNet _sentiNet;
        
        public SentenceAutoSentiment(SentiNet.SentiNet sentiNet) {
            this._sentiNet = sentiNet;
        }

        protected abstract PolarityType GetPolarity(PolarityType polarityType, AnnotatedSentence.AnnotatedSentence sentence, int index);

        private PolarityType FindPolarityType(Double sum) {
            if (sum > 0.0) {
                return PolarityType.POSITIVE;
            } else if (sum < 0.0) {
                return PolarityType.NEGATIVE;
            }
            return PolarityType.NEUTRAL;
        }

        public PolarityType AutoSentiment(AnnotatedSentence.AnnotatedSentence sentence) {
            var polarityValue = 0.0;
            for (var i = 0; i < sentence.WordCount(); i++) {
                var word = (AnnotatedWord) sentence.GetWord(i);
                try {
                    if (word != null && word.GetSemantic() != null) {
                        var sentiSynSet = _sentiNet.GetSentiSynSet(word.GetSemantic());
                        var value = System.Math.Max(sentiSynSet.GetNegativeScore(), sentiSynSet.GetPositiveScore());
                        switch (GetPolarity(sentiSynSet.GetPolarity(), sentence, i)) {
                            case PolarityType.POSITIVE:
                                polarityValue += value;
                                break;
                            case PolarityType.NEGATIVE:
                                polarityValue -= value;
                                break;
                        }
                    }
                }
                catch (Exception e) {
                    if (e is ArgumentNullException || e is KeyNotFoundException) {
                    }
                }
            }
            return FindPolarityType(polarityValue);
        }
    }
}