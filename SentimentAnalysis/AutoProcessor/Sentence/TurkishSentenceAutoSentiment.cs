using AnnotatedSentence;
using MorphologicalAnalysis;
using SentiNet;

namespace SentimentAnalysis.AutoProcessor.Sentence {
    
    public class TurkishSentenceAutoSentiment : SentenceAutoSentiment {
        
        public TurkishSentenceAutoSentiment(SentiNet.SentiNet sentiNet) : base(sentiNet) {
        }

        protected override PolarityType GetPolarity(PolarityType polarityType, AnnotatedSentence.AnnotatedSentence sentence, int index) {
            if (((AnnotatedWord) sentence.GetWord(index)).GetParse().ContainsTag(MorphologicalTag.NEGATIVE)) {
                if (polarityType.Equals(PolarityType.POSITIVE)) {
                    polarityType = PolarityType.NEGATIVE;
                } else if (polarityType.Equals(PolarityType.NEGATIVE)) {
                    polarityType = PolarityType.POSITIVE;
                }
            }
            if (index + 1 < sentence.WordCount()) {
                var nextWord = (AnnotatedWord) sentence.GetWord(index + 1);
                if (nextWord.GetParse().GetWord().GetName().ToLower().Equals("değil")) {
                    if (polarityType.Equals(PolarityType.POSITIVE)) {
                        return PolarityType.NEGATIVE;
                    } else if (polarityType.Equals(PolarityType.NEGATIVE)) {
                        return PolarityType.POSITIVE;
                    }
                }
            }
            return polarityType;
        }
    }
}